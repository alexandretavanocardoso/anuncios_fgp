using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projeto_tv.Dto;
using projeto_tv.Models;
using vansyncenterprise.web.Repositorys.Data;

namespace projeto_tv.Controllers
{
    [Authorize]
    public class AnuncioController : Controller
    {
        private readonly Context _context;
        private readonly HttpClient _client;

        public AnuncioController(Context context)
        {
            _context = context;
            _client = new HttpClient();
        }

        [HttpGet("index")]
        public async Task<ActionResult<IEnumerable<Anuncio>>> Index(int idEmpresa)
        {
            var model = await _context.Anuncio
                .AsNoTracking()
                .Where(x => x.Id == idEmpresa && DateTime.Now >= x.DataInicio && DateTime.Now <= x.DataFim)
                .ToListAsync();

            foreach (var vehicle in model)
            {
                if (vehicle.Image.Length > 0)
                    vehicle.ImageBase4 = "data:image/png;base64," + Convert.ToBase64String(vehicle.Image);
            }

            return View();
        }

        [HttpGet("historico")]
        public async Task<ActionResult<IEnumerable<Anuncio>>> Historico(int idEmpresa)
        {
            var model = await _context.Anuncio
                .AsNoTracking()
                .Where(x => x.Id == idEmpresa)
                .ToListAsync();

            foreach (var vehicle in model)
            {
                if (vehicle.Image.Length > 0)
                    vehicle.ImageBase4 = "data:image/png;base64," + Convert.ToBase64String(vehicle.Image);
            }

            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        public async Task<ActionResult> Create(int idEmpresa, AnuncioModel anuncioModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (!string.IsNullOrEmpty(anuncioModel.LinkImage))
                    anuncioModel.QrCode = await GenerateQrCode(anuncioModel.LinkImage);

                _context.Anuncio.Add(anuncioModel);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int idAnuncio)
        {
            return View();
        }

        [HttpPut("edit")]
        public async Task<ActionResult> Edit(AnuncioModel anuncioModel)
        {
            try
            {
                var existingEntity = _context.ChangeTracker.Entries<AnuncioModel>()
                                              .FirstOrDefault(e => e.Entity.Id == anuncioModel.Id);

                if (!string.IsNullOrEmpty(anuncioModel.LinkImage))
                    anuncioModel.QrCode = await GenerateQrCode(anuncioModel.LinkImage);

                if (existingEntity != null)
                    _context.Entry(existingEntity.Entity).CurrentValues.SetValues(anuncioModel);
                else
                    _context.Update(anuncioModel);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpDelete("delete")]
        public async Task<ActionResult> Delete(int idAnuncio, AnuncioModel anuncioModel)
        {
            try
            {
                _context.Anuncio.Remove(anuncioModel);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private async Task<string> GenerateQrCode(string text)
        {
            string url = $"https://chart.googleapis.com/chart?chs=300x300&cht=qr&chl={Uri.EscapeDataString(text)}";

            byte[] qrCodeBytes = await _client.GetByteArrayAsync(url);

            string base64String = Convert.ToBase64String(qrCodeBytes);
            return base64String;
        }

        public string ConvertByteArrayToBase64String(byte[] byteArray)
        {
            if (byteArray == null || byteArray.Length == 0)
                throw new ArgumentException("O array de bytes não pode ser nulo ou vazio.");

            return Convert.ToBase64String(byteArray);
        }
    }
}
