using LinqConsoleLab.EN.Data;
using LinqConsoleLab.EN.Models;

namespace LinqConsoleLab.EN.Exercises;

public sealed class LinqExercises
{
    /// <summary>
    /// Task:
    /// Find all students who live in Warsaw.
    /// Return the index number, full name, and city.
    ///
    /// SQL:
    /// SELECT IndexNumber, FirstName, LastName, City
    /// FROM Students
    /// WHERE City = 'Warsaw';
    /// </summary>
    public IEnumerable<string> Task01_StudentsFromWarsaw()
    {
        var res= UniversityData.Students
            .Where((student) => student.City.Equals("Warsaw"))
            .Select(s => new { id = s.Id, Fullname = s.FirstName + s.LastName, City = s.City });
            return res.Select((s) => s.id+" | "+s.Fullname+" | " + s.City);
    }

    /// <summary>
    /// Task:
    /// Build a list of all student email addresses.
    /// Use projection so that you do not return whole objects.
    ///
    /// SQL:
    /// SELECT Email
    /// FROM Students;
    /// </summary>
    public IEnumerable<string> Task02_StudentEmailAddresses()
    {
        return UniversityData.Students.Select((s) => s.Email);
    }

    /// <summary>
    /// Task:
    /// Sort students alphabetically by last name and then by first name.
    /// Return the index number and full name.
    ///
    /// SQL:
    /// SELECT IndexNumber, FirstName, LastName
    /// FROM Students
    /// ORDER BY LastName, FirstName;
    /// </summary>
    public IEnumerable<string> Task03_StudentsSortedAlphabetically()
    {
        var res = UniversityData.Students.OrderBy((s) => s.LastName).ThenBy((s) => s.FirstName);
            return res.Select((s)=>s.LastName+" "+s.FirstName);
    }

    /// <summary>
    /// Task:
    /// Find the first course from the Analytics category.
    /// If such a course does not exist, return a text message.
    ///
    /// SQL:
    /// SELECT TOP 1 Title, StartDate
    /// FROM Courses
    /// WHERE Category = 'Analytics';
    /// </summary>
    public IEnumerable<string> Task04_FirstAnalyticsCourse()
    {
        var res = UniversityData.Courses.Where(course => course.Category == "Analytics").FirstOrDefault();
        if (res == null)
        {
            return new string[] { "No record" };
        }

        return new string[] { res.Title + " " + res.StartDate };
    }

    /// <summary>
    /// Task:
    /// Check whether there is at least one inactive enrollment in the data set.
    /// Return one line with a True/False or Yes/No answer.
    ///
    /// SQL:
    /// SELECT CASE WHEN EXISTS (
    ///     SELECT 1
    ///     FROM Enrollments
    ///     WHERE IsActive = 0
    /// ) THEN 1 ELSE 0 END;
    /// </summary>
    public IEnumerable<string> Task05_IsThereAnyInactiveEnrollment()
    {
        var res = UniversityData.Enrollments.Where(enrollment => !enrollment.IsActive).Count()>0?"yes":"no";
        return new string[] { res };
    }

    /// <summary>
    /// Task:
    /// Check whether every lecturer has a department assigned.
    /// Use a method that validates the condition for the whole collection.
    ///
    /// SQL:
    /// SELECT CASE WHEN COUNT(*) = COUNT(Department)
    /// THEN 1 ELSE 0 END
    /// FROM Lecturers;
    /// </summary>
    public IEnumerable<string> Task06_DoAllLecturersHaveDepartment()
    {
        var res = UniversityData.Lecturers.All((l) => !l.Department.Equals(string.Empty))?"yes":"no";
        return new string[] { res };
    }

    /// <summary>
    /// Task:
    /// Count how many active enrollments exist in the system.
    ///
    /// SQL:
    /// SELECT COUNT(*)
    /// FROM Enrollments
    /// WHERE IsActive = 1;
    /// </summary>
    public IEnumerable<string> Task07_CountActiveEnrollments()
    {
        var res = UniversityData.Enrollments.Where(enrollment => enrollment.IsActive).Count();
        return new string[] { res + "" };
    }

    /// <summary>
    /// Task:
    /// Return a sorted list of distinct student cities.
    ///
    /// SQL:
    /// SELECT DISTINCT City
    /// FROM Students
    /// ORDER BY City;
    /// </summary>
    public IEnumerable<string> Task08_DistinctStudentCities()
    {
        return UniversityData.Students.Select((student => student.City)).Distinct().OrderBy(c=>c);
    }

    /// <summary>
    /// Task:
    /// Return the three newest enrollments.
    /// Show the enrollment date, student identifier, and course identifier.
    ///
    /// SQL:
    /// SELECT TOP 3 EnrollmentDate, StudentId, CourseId
    /// FROM Enrollments
    /// ORDER BY EnrollmentDate DESC;
    /// </summary>
    public IEnumerable<string> Task09_ThreeNewestEnrollments()
    {
        var res = UniversityData.Enrollments.OrderByDescending(enrollment => enrollment.EnrollmentDate).Take(3);
            return res.Select((e)=>e.EnrollmentDate+" "+e.StudentId+" "+e.CourseId+"");
        
    }

    /// <summary>
    /// Task:
    /// Implement simple pagination for the course list.
    /// Assume a page size of 2 and return the second page of data.
    ///
    /// SQL:
    /// SELECT Title, Category
    /// FROM Courses
    /// ORDER BY Title
    /// OFFSET 2 ROWS FETCH NEXT 2 ROWS ONLY;
    /// </summary>
    public IEnumerable<string> Task10_SecondPageOfCourses()
    {
        var res = UniversityData.Courses.OrderBy(e => e.Title).Skip(2).Take(2);
            return res.Select(c => $"{c.Title} ({c.Category}");
    }

    /// <summary>
    /// Task:
    /// Join students with enrollments by StudentId.
    /// Return the full student name and the enrollment date.
    ///
    /// SQL:
    /// SELECT s.FirstName, s.LastName, e.EnrollmentDate
    /// FROM Students s
    /// JOIN Enrollments e ON s.Id = e.StudentId;
    /// </summary>
    public IEnumerable<string> Task11_JoinStudentsWithEnrollments()
    {
       var res = from s in UniversityData.Students 
           join e in UniversityData.Enrollments on s.Id equals e.StudentId 
           select new {FullName=s.FirstName+ " "+s.LastName,EnrollmentDate=e.EnrollmentDate};
       return res.Select((pair)=>pair.FullName+" | " + pair.EnrollmentDate);
    }

    /// <summary>
    /// Task:
    /// Prepare all student-course pairs based on enrollments.
    /// Use an approach that flattens the data into a single result sequence.
    ///
    /// SQL:
    /// SELECT s.FirstName, s.LastName, c.Title
    /// FROM Enrollments e
    /// JOIN Students s ON s.Id = e.StudentId
    /// JOIN Courses c ON c.Id = e.CourseId;
    /// </summary>
    public IEnumerable<string> Task12_StudentCoursePairs()
    {
        var res = from e in UniversityData.Enrollments
            join s in UniversityData.Students on e.StudentId equals s.Id
            join c in UniversityData.Courses on e.CourseId equals c.Id
            select new { Fullname = s.FirstName + s.LastName, Title = c.Title };
            
        return res.Select(pair=>pair.Fullname+" | "+pair.Title);
    }

    /// <summary>
    /// Task:
    /// Group enrollments by course and return the course title together with the number of enrollments.
    ///
    /// SQL:
    /// SELECT c.Title, COUNT(*)
    /// FROM Enrollments e
    /// JOIN Courses c ON c.Id = e.CourseId
    /// GROUP BY c.Title;
    /// </summary>
    public IEnumerable<string> Task13_GroupEnrollmentsByCourse()
    {
        var res = from e in UniversityData.Enrollments
            join c in UniversityData.Courses on e.CourseId equals c.Id
            group e by c.Title
            into g
            select new {Title = g.Key, count= g.Count()};
        return res.Select(pair=>pair.Title+" | "+pair.count);
    }

    /// <summary>
    /// Task:
    /// Calculate the average final grade for each course.
    /// Ignore records where the final grade is null.
    ///
    /// SQL:
    /// SELECT c.Title, AVG(e.FinalGrade)
    /// FROM Enrollments e
    /// JOIN Courses c ON c.Id = e.CourseId
    /// WHERE e.FinalGrade IS NOT NULL
    /// GROUP BY c.Title;
    /// </summary>
    public IEnumerable<string> Task14_AverageGradePerCourse()
    {
        var res = UniversityData.Enrollments.Join(UniversityData.Courses,e=>e.CourseId,c=>c.Id,(e,c)=>new {e,c})
            .Where(pair=>pair.e.FinalGrade is not null).GroupBy(pair=>pair.c.Title)
            .Select(g=>new { Title = g.Key, Average = g.Average(pair=>pair.e.FinalGrade)});
        return res.Select(pair=>pair.Title+" | "+ pair.Average);
    }

    /// <summary>
    /// Task:
    /// For each lecturer, count how many courses are assigned to that lecturer.
    /// Return the full lecturer name and the course count.
    ///
    /// SQL:
    /// SELECT l.FirstName, l.LastName, COUNT(c.Id)
    /// FROM Lecturers l
    /// LEFT JOIN Courses c ON c.LecturerId = l.Id
    /// GROUP BY l.FirstName, l.LastName;
    /// </summary>
    public IEnumerable<string> Task15_LecturersAndCourseCounts()
    {
        var res = UniversityData.Lecturers.GroupJoin(
            UniversityData.Courses,
            l => l.Id,
            c => c.LecturerId,
            (lecturer, courseGroup) => new
            {
                FullName = $"{lecturer.FirstName} {lecturer.LastName}",
                CourseCount = courseGroup.Count()
            });
        return res.Select(pair=>pair.FullName+" "+pair.CourseCount);
    }

    /// <summary>
    /// Task:
    /// For each student, find the highest final grade.
    /// Skip students who do not have any graded enrollment yet.
    ///
    /// SQL:
    /// SELECT s.FirstName, s.LastName, MAX(e.FinalGrade)
    /// FROM Students s
    /// JOIN Enrollments e ON s.Id = e.StudentId
    /// WHERE e.FinalGrade IS NOT NULL
    /// GROUP BY s.FirstName, s.LastName;
    /// </summary>
    public IEnumerable<string> Task16_HighestGradePerStudent()
    {
        var res = UniversityData.Students.Join(
                UniversityData.Enrollments,
                s => s.Id,
                e => e.StudentId,
                (s, e) => new { s, e }).Where(pair => pair.e.FinalGrade is not null)
            .GroupBy(pair => new { pair.s.FirstName, pair.s.LastName })
            .Select(g => new { Fullname = g.Key, maxGrade = g.Max(p => p.e.FinalGrade) });
        return res.Select(r=> r.Fullname +": " +r.maxGrade);
    }

    /// <summary>
    /// Challenge:
    /// Find students who have more than one active enrollment.
    /// Return the full name and the number of active courses.
    ///
    /// SQL:
    /// SELECT s.FirstName, s.LastName, COUNT(*)
    /// FROM Students s
    /// JOIN Enrollments e ON s.Id = e.StudentId
    /// WHERE e.IsActive = 1
    /// GROUP BY s.FirstName, s.LastName
    /// HAVING COUNT(*) > 1;
    /// </summary>
    public IEnumerable<string> Challenge01_StudentsWithMoreThanOneActiveCourse()
    {
        var res = UniversityData.Students.Join(
            UniversityData.Enrollments,
            s=> s.Id,
            e => e.StudentId,
            (s, e) => new { s, e }
            ).Where(pair=>pair.e.IsActive)
            .GroupBy(pair=>pair.s.FirstName,pair=>pair.s.LastName)
            .Where(g=>g.Count()>1)
            .Select(g=> new {FullName=g.Key, Count=g.Count()});
        return res.Select(pair=>pair.FullName+" | "+pair.Count);
    }

    /// <summary>
    /// Challenge:
    /// List the courses that start in April 2026 and do not have any final grades assigned yet.
    ///
    /// SQL:
    /// SELECT c.Title
    /// FROM Courses c
    /// JOIN Enrollments e ON c.Id = e.CourseId
    /// WHERE MONTH(c.StartDate) = 4 AND YEAR(c.StartDate) = 2026
    /// GROUP BY c.Title
    /// HAVING SUM(CASE WHEN e.FinalGrade IS NOT NULL THEN 1 ELSE 0 END) = 0;
    /// </summary>
    public IEnumerable<string> Challenge02_AprilCoursesWithoutFinalGrades()
    {
        var res = UniversityData.Courses.Join(UniversityData.Enrollments,
                c => c.Id,
                e => e.CourseId,
                (c, e) => new { c, e }
            ).Where(pair => pair.c.StartDate.Month == 4 && pair.c.StartDate.Year == 2026)
            .GroupBy(pair => pair.c.Title)
            .Where(g => g.Sum(p => p.e.FinalGrade is not null ? 1 : 0) == 0);
        return res.Select(g => g.Key);
    }

    /// <summary>
    /// Challenge:
    /// Calculate the average final grade for every lecturer across all of their courses.
    /// Ignore missing grades but still keep the lecturers in mind as the reporting dimension.
    ///
    /// SQL:
    /// SELECT l.FirstName, l.LastName, AVG(e.FinalGrade)
    /// FROM Lecturers l
    /// LEFT JOIN Courses c ON c.LecturerId = l.Id
    /// LEFT JOIN Enrollments e ON e.CourseId = c.Id
    /// WHERE e.FinalGrade IS NOT NULL
    /// GROUP BY l.FirstName, l.LastName;
    /// </summary>
    public IEnumerable<string> Challenge03_LecturersAndAverageGradeAcrossTheirCourses()
    {
        var res = UniversityData.Lecturers.GroupJoin(UniversityData.Courses,
                l => l.Id,
                c => c.LecturerId,
                (lecturer, course) => new { lecturer, course }
            ).SelectMany(x => x.course.DefaultIfEmpty(),
                (l, c) => new
                {
                    lecturer = l.lecturer,
                    course = c
                }).Join(
                UniversityData.Enrollments,
                pair => pair.course.Id,
                e => e.CourseId,
                (pair, e) => new
                {
                    lecturer = pair.lecturer,
                    course = pair.course,
                    enrollment = e
                }
            ).Where(triple => triple.enrollment.FinalGrade is not null)
            .GroupBy(triple => triple.lecturer.FirstName + triple.lecturer.LastName)
            .Select(g => new
            {
                lecturer = g.Key,
                Average = g.Average(p => p.enrollment.FinalGrade)
            });
        return res.Select(obj=>obj.lecturer+" | "+obj.Average);
    }

    /// <summary>
    /// Challenge:
    /// Show student cities and the number of active enrollments created by students from each city.
    /// Sort the result by the active enrollment count in descending order.
    ///
    /// SQL:
    /// SELECT s.City, COUNT(*)
    /// FROM Students s
    /// JOIN Enrollments e ON s.Id = e.StudentId
    /// WHERE e.IsActive = 1
    /// GROUP BY s.City
    /// ORDER BY COUNT(*) DESC;
    /// </summary>
    public IEnumerable<string> Challenge04_CitiesAndActiveEnrollmentCounts()
    {
        var res = UniversityData.Students.Join(UniversityData.Enrollments,
            s => s.Id,
            e => e.StudentId,
            (s , e )=> new {student=s, enrollment=e})
            .Where(p=>p.enrollment.IsActive)
            .GroupBy(pair=>pair.student.City)
            .OrderByDescending(pair=>pair.Count());
        return res.Select(pair=>pair.Key+" | "+pair.Count());
    }

    private static NotImplementedException NotImplemented(string methodName)
    {
        return new NotImplementedException(
            $"Complete method {methodName} in Exercises/LinqExercises.cs and run the command again.");
    }
}
