using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using sanketscanffolder.Data;
using sanketscanffolder.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace sanketscanffolder.Controllers
{
    public class StudentsController : Controller
    {
        //private readonly MVCDemoDbContext _context;
        private readonly string _connectionString;

        public StudentsController(IConfiguration configuration)
        {
            //HttpContext.Session.SetInt32("StudentId", 0);
            //_context = context;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        //GET: Students
        public async Task<IActionResult> Index()
        {
            List<Student> students = new List<Student>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Students", conn);
                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    students.Add(new Student
                    {
                        Id = (int)reader["Id"],
                        Stuent_Name = reader["Stuent_Name"].ToString(),
                        Age = (int)reader["Age"],
                        Address = reader["Address"].ToString()
                    });
                }
            }
            return View(students);
        }

        // GET: Students/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var student = await _context.Students
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (student == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(student);
        //}

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Stuent_Name,Age,Address")] Student student)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    SqlCommand cmd = new SqlCommand("INSERT INTO Students (Stuent_Name, Age, Address) VALUES (@Name, @Age, @Address)", conn);
                    cmd.Parameters.AddWithValue("@Name", student.Stuent_Name);
                    cmd.Parameters.AddWithValue("@Age", student.Age);
                    cmd.Parameters.AddWithValue("@Address", student.Address);
                    await cmd.ExecuteNonQueryAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Student student = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Students WHERE Id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    student = new Student
                    {
                        Id = (int)reader["Id"],
                        Stuent_Name = reader["Stuent_Name"].ToString(),
                        Age = (int)reader["Age"],
                        Address = reader["Address"].ToString()
                    };
                }
            }

            if (student == null)
                return NotFound();

            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Stuent_Name,Age,Address")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    SqlCommand cmd = new SqlCommand("UPDATE Students SET Stuent_Name = @Name, Age = @Age, Address = @Address WHERE Id = @Id", conn);
                    cmd.Parameters.AddWithValue("@Name", student.Stuent_Name);
                    cmd.Parameters.AddWithValue("@Age", student.Age);
                    cmd.Parameters.AddWithValue("@Address", student.Address);
                    cmd.Parameters.AddWithValue("@Id", student.Id);
                    await cmd.ExecuteNonQueryAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Student student = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Students WHERE Id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    student = new Student
                    {
                        Id = (int)reader["Id"],
                        Stuent_Name = reader["Stuent_Name"].ToString(),
                        Age = (int)reader["Age"],
                        Address = reader["Address"].ToString()
                    };
                }
            }

            if (student == null)
                return NotFound();

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("DELETE FROM Students WHERE Id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                await cmd.ExecuteNonQueryAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        //private bool StudentExists(int id)
        //{
        //    return _context.Students.Any(e => e.Id == id);
        //}

        public async Task<IActionResult> GetByStudent(int studentId, string studentName)
        {
            TempData["StudentId"] = studentId;
            TempData["StudentName"] = studentName;

            List<Semester> semesters = new List<Semester>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Semesters WHERE studId = @studId", conn);
                cmd.Parameters.AddWithValue("@studId", studentId);
                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    semesters.Add(new Semester
                    {
                        SemesterId = (int)reader["SemesterId"],
                        Subjet = reader["Subjet"].ToString(),
                        Mark1 = (int)reader["Mark1"],
                        Subjet2 = reader["Subjet2"].ToString(),
                        Mark2 = (int)reader["Mark2"],
                        Total = (int)reader["Total"],
                        studId = (int)reader["studId"],
                    });
                }
            }

            return PartialView("GetByStudent", semesters);
        }
    }

    }

