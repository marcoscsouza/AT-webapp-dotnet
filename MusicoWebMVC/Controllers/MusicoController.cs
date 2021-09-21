using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace MusicoWebMVC.Controllers
{
    public class MusicoController : Controller
    {
        private readonly IMusicoRepository _musicoRepository;
        private readonly IBlobService _blobService;
        private readonly IConfiguration _configuration;
        private readonly IQueueService _queueService;

        public MusicoController(IMusicoRepository musicoRepository,
                                IBlobService blobService,
                                IConfiguration configuration,
                                IQueueService queueService)
        {
            _musicoRepository = musicoRepository;
            _blobService = blobService;
            _configuration = configuration;
            _queueService = queueService;
        }

        // GET: Musico
        public async Task<IActionResult> Index()
        {
            return View(await _musicoRepository.GetAllAsync());
        }

        // GET: Musico/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var httpClient = new HttpClient();
            //var json = JsonConvert.SerializeObject(new { musicoId = id });
            //var requestData = new StringContent(json, Encoding.UTF8, "application/json");
            //var baseAddressFunction = _configuration.GetValue<string>("FunctionBaseAddress");
            //_ = await httpClient.PostAsync(baseAddressFunction, requestData);

            var musicoModel = await _musicoRepository.GetByIdAsync(id.Value);

            var jsonMusico = JsonConvert.SerializeObject(musicoModel);
            var bytesJsonMusico = UTF8Encoding.UTF8.GetBytes(jsonMusico);
            string jsonMusicoBase64 = Convert.ToBase64String(bytesJsonMusico);

            await _queueService.SendAsync(jsonMusicoBase64);


            if (musicoModel == null)
            {
                return NotFound();
            }

            return View(musicoModel);
        }

        // GET: Musico/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Musico/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection form, MusicoModel musicoModel)
        {
            if (ModelState.IsValid)
            {
                var file = form.Files.SingleOrDefault();
                var streamFile = file.OpenReadStream();
                var uriImage = await _blobService.UploadAsync(streamFile);
                musicoModel.ImageUri = uriImage;

                await _musicoRepository.CreateAsync(musicoModel);
                return RedirectToAction(nameof(Index));
            }
            return View(musicoModel);
        }

        // GET: Musico/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var musicoModel = await _musicoRepository.GetByIdAsync(id.Value);

            if (musicoModel == null)
            {
                return NotFound();
            }
            return View(musicoModel);
        }

        // POST: Musico/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IFormCollection form, 
                                              int id, 
                                              MusicoModel musicoModel)
        {
            if (id != musicoModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var file = form.Files.SingleOrDefault();
                    var streamFile = file?.OpenReadStream();

                    if (streamFile != null)
                    {
                        var uriImage = await _blobService.UploadAsync(streamFile);

                        if (musicoModel.ImageUri != null)
                        {
                            await _blobService.DeleteAsync(musicoModel.ImageUri);
                        }

                        musicoModel.ImageUri = uriImage;
                    }

                    await _musicoRepository.EditAsync(musicoModel);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MusicoModelExists(musicoModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(musicoModel);
        }

        // GET: Musico/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var musicoModel = await _musicoRepository.GetByIdAsync(id.Value);

            if (musicoModel == null)
            {
                return NotFound();
            }

            return View(musicoModel);
        }

        // POST: Musico/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var musico = _musicoRepository.GetByIdAsync(id).Result;

            if (musico.ImageUri != null)
            {
                await _blobService.DeleteAsync(musico.ImageUri);
            }

            await _musicoRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private bool MusicoModelExists(int id)
        {
            var musico = _musicoRepository.GetByIdAsync(id).Result;

            return (musico != null);
        }
    }
}
