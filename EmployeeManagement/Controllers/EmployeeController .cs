using EmployeeManagement.BLL;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic; 
using System.IO;
using QuestPDF.Fluent; 
using QuestPDF.Helpers;
using QuestPDF.Previewer; 
using System.Linq; 

namespace EmployeeManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _service;

    public EmployeeController(IEmployeeService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetAll() => Ok(_service.GetAll());

    [HttpGet("states")]
    public IActionResult GetAllStates() => Ok(_service.GetAllStates());

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var emp = _service.Get(id);
        return emp == null ? NotFound() : Ok(emp);
    }

    [HttpPost]
    public IActionResult Create([FromBody] Employee emp) 
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); 
        }

        try
        {
            _service.ValidateAndCreate(emp);
            return Ok(); 
        }
        catch (InvalidOperationException ex)
        {
          
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred during creation.", error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Employee emp) 
    {
        if (id != emp.Id)
        {
            return BadRequest(new { message = "ID mismatch." });
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); 
        }

        try
        {
            _service.ValidateAndEdit(emp);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred during update.", error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        try
        {
            _service.Remove(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred during deletion.", error = ex.Message });
        }
    }

   
    [HttpDelete("bulk-delete")]
    public IActionResult DeleteMultiple([FromBody] List<int> ids)
    {
        if (ids == null || !ids.Any())
        {
            return BadRequest(new { message = "No employee IDs provided for deletion." });
        }

        try
        {
            _service.RemoveMultiple(ids);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"An error occurred during bulk deletion: {ex.Message}" });
        }
    }

    [HttpGet("download-pdf")]
    public IActionResult DownloadPdf()
    {
        try
        {
            var employees = _service.GetAll();

            if (!employees.Any())
            {
                return NotFound(new { message = "No employee data available to generate PDF." });
            }

            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(50);
                    page.Header().Text("Employee List").Bold().FontSize(20).AlignCenter();

                    page.Content().Column(column =>
                    {
                        column.Spacing(10);

                        column.Item().Table(table =>
                        {
                           
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(1.2f); 
                                columns.RelativeColumn(1);    
                                columns.RelativeColumn(0.8f); 
                                columns.RelativeColumn(0.8f); 
                                columns.RelativeColumn(0.7f); 
                                columns.RelativeColumn(0.5f); 
                                columns.RelativeColumn(0.8f); 
                            });

                            
                            table.Header(header =>
                            {
                                header.Cell().Border(1).Padding(5).Background(Colors.Grey.Lighten2).AlignMiddle().AlignCenter().Text("Name").Bold();
                                header.Cell().Border(1).Padding(5).Background(Colors.Grey.Lighten2).AlignMiddle().AlignCenter().Text("Designation").Bold();
                                header.Cell().Border(1).Padding(5).Background(Colors.Grey.Lighten2).AlignMiddle().AlignCenter().Text("DOJ").Bold();
                                header.Cell().Border(1).Padding(5).Background(Colors.Grey.Lighten2).AlignMiddle().AlignCenter().Text("DOB").Bold();
                                header.Cell().Border(1).Padding(5).Background(Colors.Grey.Lighten2).AlignMiddle().AlignRight().Text("Salary").Bold(); // Align salary header right
                                header.Cell().Border(1).Padding(5).Background(Colors.Grey.Lighten2).AlignMiddle().AlignCenter().Text("Gender").Bold();
                                header.Cell().Border(1).Padding(5).Background(Colors.Grey.Lighten2).AlignMiddle().AlignCenter().Text("State").Bold();
                            });

                           
                            foreach (var emp in employees)
                            {
                                table.Cell().Border(1).Padding(5).AlignMiddle().Text(emp.Name);
                                table.Cell().Border(1).Padding(5).AlignMiddle().Text(emp.Designation);
                                table.Cell().Border(1).Padding(5).AlignMiddle().Text(emp.DateOfJoin.ToString("dd-MMM-yyyy"));
                                table.Cell().Border(1).Padding(5).AlignMiddle().Text(emp.DateOfBirth.ToString("dd-MMM-yyyy"));
                                table.Cell().Border(1).Padding(5).AlignMiddle().AlignRight().Text($"₹ {emp.Salary:N0}"); // Align salary data right
                                table.Cell().Border(1).Padding(5).AlignMiddle().Text(emp.Gender);
                                table.Cell().Border(1).Padding(5).AlignMiddle().Text(emp.State);
                            }
                        });
                    });

                    page.Footer().Text(x =>
                    {
                        x.Span("Page ").FontSize(10);
                        x.CurrentPageNumber().FontSize(10);
                        x.Span(" of ").FontSize(10);
                        x.TotalPages().FontSize(10);
                    });
                });
            }).GeneratePdf(); 

            return File(pdf, "application/pdf", "EmployeeList.pdf");
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Error generating PDF: {ex.Message}" });
        }
    }
}