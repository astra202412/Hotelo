using System.Text;
using ClosedXML.Excel;
using Hotelo.Core.Interfaces.Repositories;
using Hotelo.Core.Interfaces.Services;

namespace Hotelo.Infrastructure.Services;

public class ExportService : IExportService
{
    private readonly IInvoiceRepository _invoiceRepo;
    private readonly IReservationRepository _resRepo;
    private readonly IRoomRepository _roomRepo;
    private readonly IGuestRepository _guestRepo;

    public ExportService(IInvoiceRepository invoiceRepo,
                         IReservationRepository resRepo,
                         IRoomRepository roomRepo,
                         IGuestRepository guestRepo)
    {
        _invoiceRepo = invoiceRepo;
        _resRepo = resRepo;
        _roomRepo = roomRepo;
        _guestRepo = guestRepo;
    }

    // ── PDF Facture ──────────────────────────────────────────────────────────
    public async Task<byte[]> ExportInvoicePdfAsync(int invoiceId)
    {
        var invoice = await _invoiceRepo.GetWithDetailsAsync(invoiceId)
            ?? throw new KeyNotFoundException($"Facture {invoiceId} introuvable");

        var nights = invoice.Reservation != null
            ? (invoice.Reservation.CheckOut - invoice.Reservation.CheckIn).Days
            : 0;

        var html = $@"<!DOCTYPE html>
<html>
<head>
<meta charset='utf-8'>
<style>
  body {{ font-family: Arial, sans-serif; margin: 40px; color: #333; }}
  .header {{ display: flex; justify-content: space-between; border-bottom: 3px solid #1E3A5F; padding-bottom: 20px; margin-bottom: 30px; }}
  .brand {{ font-size: 36px; font-weight: 900; color: #1E3A5F; letter-spacing: 3px; }}
  .brand small {{ display: block; font-size: 12px; color: #888; font-weight: normal; letter-spacing: 1px; }}
  .invoice-title {{ font-size: 22px; font-weight: bold; color: #1E3A5F; margin-bottom: 20px; }}
  .info-grid {{ display: grid; grid-template-columns: 1fr 1fr; gap: 20px; margin-bottom: 30px; }}
  .info-box {{ background: #f5f5f5; padding: 15px; border-radius: 8px; }}
  .info-box h4 {{ color: #1E3A5F; font-size: 13px; margin: 0 0 8px 0; text-transform: uppercase; }}
  .info-box p {{ margin: 3px 0; font-size: 14px; }}
  table {{ width: 100%; border-collapse: collapse; margin-bottom: 20px; }}
  th {{ background: #1E3A5F; color: white; padding: 10px; text-align: left; font-size: 13px; }}
  td {{ padding: 10px; border-bottom: 1px solid #eee; font-size: 13px; }}
  .totals {{ margin-left: auto; width: 300px; }}
  .totals tr td {{ border: none; padding: 5px 10px; }}
  .totals tr.total-ttc td {{ font-weight: bold; font-size: 16px; color: #1E3A5F; border-top: 2px solid #1E3A5F; }}
  .badge {{ padding: 4px 10px; border-radius: 20px; font-size: 12px; color: white;
            background: {(invoice.IsPaid ? "#4CAF50" : "#F44336")}; }}
  .footer {{ margin-top: 40px; border-top: 1px solid #eee; padding-top: 15px; text-align: center; color: #888; font-size: 11px; }}
</style>
</head>
<body>
<div class='header'>
  <div>
    <div class='brand'>HOTELO <small>Hotel Management System</small></div>
  </div>
  <div style='text-align:right'>
    <div style='font-size:20px;font-weight:bold;color:#C9A84C'>{invoice.InvoiceNumber}</div>
    <div style='font-size:13px;color:#888'>Date : {invoice.CreatedAt:dd/MM/yyyy}</div>
    <div style='margin-top:5px'><span class='badge'>{(invoice.IsPaid ? "PAYEE" : "IMPAYEE")}</span></div>
  </div>
</div>

<div class='invoice-title'>FACTURE DE SEJOUR</div>

<div class='info-grid'>
  <div class='info-box'>
    <h4>Client</h4>
    <p><strong>{invoice.Reservation?.Guest?.FullName}</strong></p>
    <p>{invoice.Reservation?.Guest?.Phone}</p>
    <p>{invoice.Reservation?.Guest?.Email}</p>
    <p>Passeport : {invoice.Reservation?.Guest?.Passport ?? "-"}</p>
  </div>
  <div class='info-box'>
    <h4>Details du Sejour</h4>
    <p>Chambre : <strong>{invoice.Reservation?.Room?.RoomNumber} — {invoice.Reservation?.Room?.RoomType?.Name}</strong></p>
    <p>Arrivee  : {invoice.Reservation?.CheckIn:dd/MM/yyyy}</p>
    <p>Depart   : {invoice.Reservation?.CheckOut:dd/MM/yyyy}</p>
    <p>Duree    : <strong>{nights} nuit(s)</strong></p>
    <p>Code     : {invoice.Reservation?.ConfirmationCode}</p>
  </div>
</div>

<table>
  <thead>
    <tr><th>Designation</th><th>Prix Unitaire</th><th>Quantite</th><th>Total</th></tr>
  </thead>
  <tbody>
    <tr>
      <td>Hebergement — {invoice.Reservation?.Room?.RoomType?.Name}</td>
      <td>{invoice.Reservation?.RoomRate:N0} DZD</td>
      <td>{nights} nuit(s)</td>
      <td>{(invoice.Reservation?.RoomRate * nights):N0} DZD</td>
    </tr>
    {(invoice.Reservation?.PackagePrice > 0 ? $@"
    <tr>
      <td>Package</td>
      <td>{invoice.Reservation?.PackagePrice:N0} DZD</td>
      <td>{nights} nuit(s)</td>
      <td>{(invoice.Reservation?.PackagePrice * nights):N0} DZD</td>
    </tr>" : "")}
    {(invoice.Discount > 0 ? $@"
    <tr>
      <td colspan='3' style='color:#F44336'>Remise</td>
      <td style='color:#F44336'>- {invoice.Discount:N0} DZD</td>
    </tr>" : "")}
  </tbody>
</table>

<table class='totals'>
  <tr><td>Total HT</td><td style='text-align:right'>{invoice.TotalHT:N0} DZD</td></tr>
  <tr><td>TVA ({invoice.TVARate * 100:0}%)</td><td style='text-align:right'>{invoice.TVAAmount:N0} DZD</td></tr>
  <tr class='total-ttc'><td>TOTAL TTC</td><td style='text-align:right'>{invoice.TotalTTC:N0} DZD</td></tr>
</table>

{(invoice.Payments?.Any() == true ? $@"
<div style='margin-top:20px;padding:15px;background:#E8F5E9;border-radius:8px;'>
  <strong style='color:#2E7D32'>Paiements Reçus :</strong>
  {string.Join("", invoice.Payments.Select(p => $"<div style='font-size:13px;margin-top:5px'>• {p.Method} — {p.Amount:N0} DZD — {p.PaidAt:dd/MM/yyyy}</div>"))}
</div>" : "")}

<div class='footer'>
  HOTELO HMS — Merci de votre confiance<br>
  Ce document est une facture officielle generee automatiquement.
</div>
</body>
</html>";

        return Encoding.UTF8.GetBytes(html);
    }

    // ── Excel Reservations ────────────────────────────────────────────────────
    public async Task<byte[]> ExportReservationsExcelAsync(DateTime from, DateTime to)
    {
        var reservations = (await _resRepo.GetByDateRangeAsync(from, to)).ToList();

        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("Reservations");

        // Style en-tete
        var headerStyle = ws.Range("A1:J1");
        ws.Cell("A1").Value = "Code Confirmation";
        ws.Cell("B1").Value = "Client";
        ws.Cell("C1").Value = "Chambre";
        ws.Cell("D1").Value = "Type";
        ws.Cell("E1").Value = "Arrivee";
        ws.Cell("F1").Value = "Depart";
        ws.Cell("G1").Value = "Nuits";
        ws.Cell("H1").Value = "Statut";
        ws.Cell("I1").Value = "Total (DZD)";
        ws.Cell("J1").Value = "Date Creation";

        var header = ws.Range("A1:J1");
        header.Style.Fill.BackgroundColor = XLColor.FromHtml("#1E3A5F");
        header.Style.Font.FontColor = XLColor.White;
        header.Style.Font.Bold = true;
        header.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        var row = 2;
        foreach (var r in reservations)
        {
            ws.Cell(row, 1).Value = r.ConfirmationCode;
            ws.Cell(row, 2).Value = r.Guest?.FullName;
            ws.Cell(row, 3).Value = r.Room?.RoomNumber;
            ws.Cell(row, 4).Value = r.Room?.RoomType?.Name;
            ws.Cell(row, 5).Value = r.CheckIn.ToString("dd/MM/yyyy");
            ws.Cell(row, 6).Value = r.CheckOut.ToString("dd/MM/yyyy");
            ws.Cell(row, 7).Value = (r.CheckOut - r.CheckIn).Days;
            ws.Cell(row, 8).Value = r.Status.ToString();
            ws.Cell(row, 9).Value = (double)r.TotalAmount;
            ws.Cell(row, 10).Value = r.CreatedAt.ToString("dd/MM/yyyy");

            if (row % 2 == 0)
                ws.Range(row, 1, row, 10).Style.Fill.BackgroundColor = XLColor.FromHtml("#EBF3FB");
            row++;
        }

        ws.Columns().AdjustToContents();
        ws.Range(1, 9, row - 1, 9).Style.NumberFormat.Format = "#,##0 DZD";

        // Ligne totaux
        ws.Cell(row, 8).Value = "TOTAL";
        ws.Cell(row, 8).Style.Font.Bold = true;
        ws.Cell(row, 9).FormulaA1 = $"=SUM(I2:I{row - 1})";
        ws.Cell(row, 9).Style.Font.Bold = true;
        ws.Cell(row, 9).Style.NumberFormat.Format = "#,##0 DZD";

        using var ms = new MemoryStream();
        wb.SaveAs(ms);
        return ms.ToArray();
    }

    // ── Excel Finances ────────────────────────────────────────────────────────
    public async Task<byte[]> ExportFinancesExcelAsync(DateTime from, DateTime to)
    {
        var invoices = (await _invoiceRepo.GetAllWithDetailsAsync())
            .Where(i => i.CreatedAt >= from && i.CreatedAt <= to)
            .ToList();

        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("Finances");

        string[] headers = { "Numero", "Client", "Chambre", "Date", "Total HT", "TVA", "Total TTC", "Statut", "Date Paiement" };
        for (int i = 0; i < headers.Length; i++)
            ws.Cell(1, i + 1).Value = headers[i];

        var header = ws.Range(1, 1, 1, headers.Length);
        header.Style.Fill.BackgroundColor = XLColor.FromHtml("#1E3A5F");
        header.Style.Font.FontColor = XLColor.White;
        header.Style.Font.Bold = true;

        var row = 2;
        foreach (var inv in invoices)
        {
            ws.Cell(row, 1).Value = inv.InvoiceNumber;
            ws.Cell(row, 2).Value = inv.Reservation?.Guest?.FullName;
            ws.Cell(row, 3).Value = inv.Reservation?.Room?.RoomNumber;
            ws.Cell(row, 4).Value = inv.CreatedAt.ToString("dd/MM/yyyy");
            ws.Cell(row, 5).Value = (double)inv.TotalHT;
            ws.Cell(row, 6).Value = (double)inv.TVAAmount;
            ws.Cell(row, 7).Value = (double)inv.TotalTTC;
            ws.Cell(row, 8).Value = inv.IsPaid ? "Payee" : "Impayee";
            ws.Cell(row, 9).Value = inv.PaidAt?.ToString("dd/MM/yyyy") ?? "-";

            if (!inv.IsPaid)
                ws.Cell(row, 8).Style.Font.FontColor = XLColor.Red;
            else
                ws.Cell(row, 8).Style.Font.FontColor = XLColor.FromHtml("#2E7D32");

            if (row % 2 == 0)
                ws.Range(row, 1, row, headers.Length).Style.Fill.BackgroundColor = XLColor.FromHtml("#EBF3FB");
            row++;
        }

        ws.Columns().AdjustToContents();
        foreach (int col in new[] { 5, 6, 7 })
            ws.Range(2, col, row - 1, col).Style.NumberFormat.Format = "#,##0";

        // Totaux
        ws.Cell(row, 4).Value = "TOTAUX";
        ws.Cell(row, 4).Style.Font.Bold = true;
        ws.Cell(row, 5).FormulaA1 = $"=SUM(E2:E{row - 1})";
        ws.Cell(row, 6).FormulaA1 = $"=SUM(F2:F{row - 1})";
        ws.Cell(row, 7).FormulaA1 = $"=SUM(G2:G{row - 1})";
        ws.Range(row, 4, row, 7).Style.Font.Bold = true;
        ws.Range(row, 4, row, 7).Style.Fill.BackgroundColor = XLColor.FromHtml("#E8F5E9");

        using var ms = new MemoryStream();
        wb.SaveAs(ms);
        return ms.ToArray();
    }

    // ── Excel Chambres ────────────────────────────────────────────────────────
    public async Task<byte[]> ExportRoomsExcelAsync()
    {
        var rooms = (await _roomRepo.GetAllWithDetailsAsync()).ToList();

        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("Chambres");

        string[] headers = { "Numero", "Type", "Etage", "Statut", "Prix/Nuit (DZD)", "Capacite", "Actif" };
        for (int i = 0; i < headers.Length; i++)
            ws.Cell(1, i + 1).Value = headers[i];

        var header = ws.Range(1, 1, 1, headers.Length);
        header.Style.Fill.BackgroundColor = XLColor.FromHtml("#1E3A5F");
        header.Style.Font.FontColor = XLColor.White;
        header.Style.Font.Bold = true;

        var row = 2;
        foreach (var r in rooms)
        {
            ws.Cell(row, 1).Value = r.RoomNumber;
            ws.Cell(row, 2).Value = r.RoomType?.Name;
            ws.Cell(row, 3).Value = r.Floor?.Name;
            ws.Cell(row, 4).Value = r.Status.ToString();
            ws.Cell(row, 5).Value = (double)(r.RoomType?.BasePrice ?? 0);
            ws.Cell(row, 6).Value = r.RoomType?.MaxOccupancy ?? 0;
            ws.Cell(row, 7).Value = r.IsActive ? "Oui" : "Non";
            if (row % 2 == 0)
                ws.Range(row, 1, row, headers.Length).Style.Fill.BackgroundColor = XLColor.FromHtml("#EBF3FB");
            row++;
        }
        ws.Columns().AdjustToContents();

        using var ms = new MemoryStream();
        wb.SaveAs(ms);
        return ms.ToArray();
    }

    // ── Excel Clients ─────────────────────────────────────────────────────────
    public async Task<byte[]> ExportGuestsExcelAsync()
    {
        var guests = (await _guestRepo.GetWithReservationsAsync()).ToList();

        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("Clients");

        string[] headers = { "Nom Complet", "Passeport", "Nationalite", "Email", "Telephone", "VIP", "Nb Sejours" };
        for (int i = 0; i < headers.Length; i++)
            ws.Cell(1, i + 1).Value = headers[i];

        var header = ws.Range(1, 1, 1, headers.Length);
        header.Style.Fill.BackgroundColor = XLColor.FromHtml("#1E3A5F");
        header.Style.Font.FontColor = XLColor.White;
        header.Style.Font.Bold = true;

        var row = 2;
        foreach (var g in guests)
        {
            ws.Cell(row, 1).Value = g.FullName;
            ws.Cell(row, 2).Value = g.Passport ?? "-";
            ws.Cell(row, 3).Value = g.Nationality ?? "-";
            ws.Cell(row, 4).Value = g.Email ?? "-";
            ws.Cell(row, 5).Value = g.Phone ?? "-";
            ws.Cell(row, 6).Value = g.VIPLevel switch { 1 => "Silver", 2 => "Gold", 3 => "Platinum", _ => "Standard" };
            ws.Cell(row, 7).Value = g.Reservations?.Count ?? 0;
            if (row % 2 == 0)
                ws.Range(row, 1, row, headers.Length).Style.Fill.BackgroundColor = XLColor.FromHtml("#EBF3FB");
            row++;
        }
        ws.Columns().AdjustToContents();

        using var ms = new MemoryStream();
        wb.SaveAs(ms);
        return ms.ToArray();
    }
}
