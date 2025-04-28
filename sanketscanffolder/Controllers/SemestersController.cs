using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using sanketscanffolder.Data;
using sanketscanffolder.Models;

namespace sanketscanffolder.Controllers
{
    public class SemestersController : Controller
    {
        private readonly string _connectionString;

        public SemestersController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // GET: Semesters
        public async Task<IActionResult> Index()
        {
            List<Semester> semesters = new List<Semester>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Semesters", conn);
                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    semesters.Add(new Semester
                    {
                        SemesterId = (int)reader["SemesterId"],
                        studId = (int)reader["studId"],
                        Subjet = reader["Subjet"].ToString(),
                        Mark1 = (int)reader["Mark1"],
                        Subjet2 = reader["Subjet2"].ToString(),
                        Mark2 = (int)reader["Mark2"],
                        Total = (int)reader["Total"]
                    });
                }
            }
            return View(semesters);
        }

        //// GET: Semesters/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var semester = await _context.Semesters
        //        .FirstOrDefaultAsync(m => m.SemesterId == id);
        //    if (semester == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(semester);
        //}

        // GET: Semesters/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Semesters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: Semesters/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SemesterId,Subjet,Mark1,Subjet2,Mark2,Total")] Semester semester)
        {
            if (ModelState.IsValid)
            {
                semester.studId = Convert.ToInt32(TempData["StudentId"]);
                semester.Total = semester.Mark1 + semester.Mark2;

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    SqlCommand cmd = new SqlCommand("INSERT INTO Semesters (studId, Subjet, Mark1, Subjet2, Mark2, Total) VALUES (@studId, @Subjet, @Mark1, @Subjet2, @Mark2, @Total)", conn);
                    cmd.Parameters.AddWithValue("@studId", semester.studId);
                    cmd.Parameters.AddWithValue("@Subjet", semester.Subjet);
                    cmd.Parameters.AddWithValue("@Mark1", semester.Mark1);
                    cmd.Parameters.AddWithValue("@Subjet2", semester.Subjet2);
                    cmd.Parameters.AddWithValue("@Mark2", semester.Mark2);
                    cmd.Parameters.AddWithValue("@Total", semester.Total);
                    await cmd.ExecuteNonQueryAsync();
                }
                return RedirectToAction("Index", "Students");
            }
            return View(semester);
        }


        // GET: Semesters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            Semester semester = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Semesters WHERE SemesterId = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    semester = new Semester
                    {
                        SemesterId = (int)reader["SemesterId"],
                        studId = (int)reader["studId"],
                        Subjet = reader["Subjet"].ToString(),
                        Mark1 = (int)reader["Mark1"],
                        Subjet2 = reader["Subjet2"].ToString(),
                        Mark2 = (int)reader["Mark2"],
                        Total = (int)reader["Total"]
                    };
                }
            }

            if (semester == null)
                return NotFound();

            return View(semester);
        }
        // POST: Semesters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: Semesters/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SemesterId,Subjet,Mark1,Subjet2,Mark2,Total")] Semester semester)
        {
            if (id != semester.SemesterId)
                return NotFound();

            if (ModelState.IsValid)
            {
                semester.studId = Convert.ToInt32(TempData["StudentId"]);
                semester.Total = semester.Mark1 + semester.Mark2;

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    SqlCommand cmd = new SqlCommand("UPDATE Semesters SET studId = @studId, Subjet = @Subjet, Mark1 = @Mark1, Subjet2 = @Subjet2, Mark2 = @Mark2, Total = @Total WHERE SemesterId = @SemesterId", conn);
                    cmd.Parameters.AddWithValue("@studId", semester.studId);
                    cmd.Parameters.AddWithValue("@Subjet", semester.Subjet);
                    cmd.Parameters.AddWithValue("@Mark1", semester.Mark1);
                    cmd.Parameters.AddWithValue("@Subjet2", semester.Subjet2);
                    cmd.Parameters.AddWithValue("@Mark2", semester.Mark2);
                    cmd.Parameters.AddWithValue("@Total", semester.Total);
                    cmd.Parameters.AddWithValue("@SemesterId", semester.SemesterId);
                    await cmd.ExecuteNonQueryAsync();
                }
                return RedirectToAction("Index", "Students");
            }
            return View(semester);
        }


        // GET: Semesters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            Semester semester = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Semesters WHERE SemesterId = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    semester = new Semester
                    {
                        SemesterId = (int)reader["SemesterId"],
                        studId = (int)reader["studId"],
                        Subjet = reader["Subjet"].ToString(),
                        Mark1 = (int)reader["Mark1"],
                        Subjet2 = reader["Subjet2"].ToString(),
                        Mark2 = (int)reader["Mark2"],
                        Total = (int)reader["Total"]
                    };
                }
            }

            if (semester == null)
                return NotFound();

            return View(semester);
        }

        // POST: Semesters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("DELETE FROM Semesters WHERE SemesterId = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                await cmd.ExecuteNonQueryAsync();
            }
            return RedirectToAction("Index", "Students");
        }

        //private bool SemesterExists(int id)
        //{
        //    return _context.Semesters.Any(e => e.SemesterId == id);
        //}


    }
}
