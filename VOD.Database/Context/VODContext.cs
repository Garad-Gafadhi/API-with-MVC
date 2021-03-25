using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VOD.Common.Entities;

namespace VOD.Database.Context
{
    public class VodContext : IdentityDbContext<VodUser>
    {
        public VodContext(DbContextOptions<VodContext> options) : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Download> Downloads { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<UserCourse> UserCourses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            SeedData(builder);

            //Composite key
            builder.Entity<UserCourse>().HasKey(uc => new
            {
                uc.UserId,
                uc.CourseId
            });
            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }

        #region creating a user and userRoles

        private void SeedData(ModelBuilder builder)
        {
            var email = "a.b@C";
            var password = "Test123__";
            var user = new VodUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = email,
                NormalizedEmail = email.ToUpper(),
                UserName = email,
                NormalizedUserName = email.ToUpper(),
                EmailConfirmed = true
            };

            var passwordhasher = new PasswordHasher<VodUser>();
            user.PasswordHash = passwordhasher.HashPassword(user, password);
            builder.Entity<VodUser>().HasData(user);

            //
            var admin = "Admin";
            var role = new IdentityRole
            {
                Id = "1",
                Name = admin,
                NormalizedName = admin
            };

            builder.Entity<IdentityRole>().HasData(role);

            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = role.Id,
                    UserId = user.Id
                });

            //Claims

            builder.Entity<IdentityUserClaim<string>>().HasData(
                new IdentityUserClaim<string>
                {
                    Id = 1,
                    ClaimType = admin,
                    ClaimValue = "true",
                    UserId = user.Id
                });

            builder.Entity<IdentityUserClaim<string>>().HasData(
                new IdentityUserClaim<string>
                {
                    Id = 2,
                    ClaimType = "VODUser",
                    ClaimValue = "true",
                    UserId = user.Id
                });
        }

        #endregion creating a user and userRoles


        public void SeedMembershipData()
        {
            var description1 =
                "C# is a simple, modern, general-purpose, object-oriented programming language" +
                " developed by Microsoft within its .NET initiative led by Anders Hejlsberg.";
            var description2 = "JavaScript is a fun and flexible programming language." +
                               " It's one of the core technologies of web development" +
                               " and can be used on both the front-end and the ..";
            var description3 = "HTML is the World Wide Web's core markup language. Originally," +
                               " HTML was primarily designed as a language for semantically describing ";

            #region Fetch a User

            var email = "a.b@c";
            var userId = string.Empty;

            if (Users.Any(r => r.Email.Equals(email)))
                userId = Users.First(r => r.Email.Equals(email)).Id;
            else
                return;

            #endregion Fetch a User

            #region Add Instructors if they don't already exist

            if (!Instructors.Any())
            {
                var instructors = new List<Instructor>
                {
                    new Instructor
                    {
                        Name = "Garad Gafadi",
                        Description =
                            "Garad Gafadi is a software developer and a future co-founder of OneLove Corporation.",

                        Thumbnail = "/images/avatar2.jpg"
                    },
                    new Instructor
                    {
                        Name = "Adam Ganays",
                        Description = "Adam is a software developer and a future co-founder of OneLove Corporation.",
                        Thumbnail = " /Images/avatar.png"
                    },
                    new Instructor
                    {
                        Name = "Lovisa Jonsson",
                        Description = "Adam is a software developer and a future co-founder of OneLove Corporation.",
                        Thumbnail = " /Images/avatar.png"
                    }
                };
                Instructors.AddRange(instructors);
                SaveChanges();
            }

            if (Instructors.Count() < 3) return;

            #endregion Add Instructors if they don't already exist

            #region Add Courses if they don't already exist

            if (!Courses.Any())
            {
                var instructorId1 = Instructors.First().Id;
                var instructorId2 = Instructors.Skip(1).FirstOrDefault().Id;
                var instructorId3 = Instructors.Skip(2).FirstOrDefault().Id;

                var courses = new List<Course>
                {
                    new Course
                    {
                        InstructorId = instructorId1,
                        Title = "C#",
                        Description = description1,
                        ImageUrl = "/Images/cours1.jpg",
                        MarqueeImageUrl = "/Images/laptop.jpg"
                    },
                    new Course
                    {
                        InstructorId = instructorId2,
                        Title = "JavaScript",
                        Description = description2,
                        ImageUrl = "/Images/cours2.png",
                        MarqueeImageUrl = "/images/laptop.jpg"
                    },
                    new Course
                    {
                        InstructorId = instructorId3,
                        Title = "HMTL",
                        Description = description3,
                        ImageUrl = "/Images/cours3.png",
                        MarqueeImageUrl = "/Images/laptop.jpg"
                    }
                };
                Courses.AddRange(courses);
                SaveChanges();
            }

            if (Courses.Count() < 3) return;

            #endregion Add Courses if they don't already exist

            #region Fetch Course ids if any courses exists

            var courseId1 = Courses.First().Id;
            var courseId2 = Courses.Skip(1).FirstOrDefault().Id;
            var courseId3 = Courses.Skip(2).FirstOrDefault().Id;

            #endregion Fetch Course ids if any courses exists

            #region Add UserCourses connections if they don't already exist

            if (!UserCourses.Any())
            {
                UserCourses.Add(new UserCourse {UserId = userId, CourseId = courseId1});
                UserCourses.Add(new UserCourse {UserId = userId, CourseId = courseId2});
                UserCourses.Add(new UserCourse {UserId = userId, CourseId = courseId3});

                SaveChanges();
            }

            if (UserCourses.Count() < 3) return;

            #endregion Add UserCourses connections if they don't already exist

            #region Add Modules if they don't already exist

            if (!Modules.Any())
            {
                var modules = new List<Module>
                {
                    new Module
                    {
                        Course = Find<Course>(courseId1), Title = "Module 1"
                    },

                    new Module
                    {
                        Course = Find<Course>(courseId2), Title = "Module 2"
                    },
                    new Module
                    {
                        Course = Find<Course>(courseId3), Title = "Module 3"
                    }
                };
                Modules.AddRange(modules);
                SaveChanges();
            }

            if (Modules.Count() < 3) return;

            #endregion Add Modules if they don't already exist

            #region Fetch Module ids if any modules exist

            var moduleId1 = Modules.First().Id;
            var moduleId2 = Modules.Skip(1).FirstOrDefault().Id;
            var moduleId3 = Modules.Skip(2).FirstOrDefault().Id;

            #endregion Fetch Module ids if any modules exist

            #region Add Videos if they don't already exist

            if (!Videos.Any())
            {
                var video = new List<Video>
                {
                    new Video
                    {
                        ModuleId = moduleId1, CourseId = courseId1, Title = "C# Toturial1",
                        Description = description1.Substring(1, 100),
                        Thumbnail = "/images/video1.jpg",
                        Duration = 14, Url = "https://youtu.be/yK6zlTVqWzo"
                    },

                    new Video
                    {
                        ModuleId = moduleId1, CourseId = courseId1, Title = "JavaScript For Beginers",
                        Description = description2.Substring(1, 50),
                        Thumbnail = "/images/video2.jpg",
                        Duration = 48, Url = "https://www.youtube.com/watch?v=qv6ZflueASY"
                    },

                    new Video
                    {
                        ModuleId = moduleId1, CourseId = courseId1, Title = "HTML For Beginers",
                        Description = description3.Substring(1, 80),
                        Thumbnail = "/images/video3.jpg",
                        Duration = 12, Url = "https://youtu.be/bWPMSSsVdPk"
                    },

                    new Video
                    {
                        ModuleId = moduleId2, CourseId = courseId2, Title = "JavaScript For Beginers",
                        Description = description2.Substring(1, 50),
                        Thumbnail = "/images/video2.jpg",
                        Duration = 48, Url = "https://youtu.be/W6NZfCO5SIk"
                    },

                    new Video
                    {
                        ModuleId = moduleId2, CourseId = courseId2, Title = "HTML For Beginers",
                        Description = description3.Substring(1, 80),
                        Thumbnail = "/images/video3.jpg",
                        Duration = 12, Url = "https://youtu.be/bWPMSSsVdPk"
                    },
                    new Video
                    {
                        ModuleId = moduleId3, CourseId = courseId2, Title = "JavaScript For Beginers",
                        Description = description2.Substring(1, 50),
                        Thumbnail = "/images/video2.jpg",
                        Duration = 48, Url = "https://youtu.be/W6NZfCO5SIk"
                    },

                    new Video
                    {
                        ModuleId = moduleId3, CourseId = courseId2, Title = "HTML For Beginers",
                        Description = description3.Substring(1, 80),
                        Thumbnail = "/images/video3.jpg",
                        Duration = 12, Url = "https://youtu.be/bWPMSSsVdPk"
                    }
                };
                Videos.AddRange(video);
                SaveChanges();
            }

            #endregion Add Videos if they don't already exist

            #region Add Downloads if they don't already exist

            if (!Downloads.Any())
            {
                var downloads = new List<Download>
                {
                    new Download
                    {
                        ModuleId = moduleId1, CourseId = courseId1, Title = "C# Toturials (PDF)",
                        Url = "https://some-url"
                    },
                    new Download
                    {
                        ModuleId = moduleId1, CourseId = courseId1, Title = "JavaScript Toturials (PDF)",
                        Url = "https://some-url"
                    },
                    new Download
                    {
                        ModuleId = moduleId1, CourseId = courseId1, Title = "HTML Toturials (PDF)",
                        Url = "https://some-url"
                    },
                    new Download
                    {
                        ModuleId = moduleId1, CourseId = courseId2, Title = "JavaScript Toturials (PDF)",
                        Url = "https://some-url"
                    },
                    new Download
                    {
                        ModuleId = moduleId1, CourseId = courseId2, Title = "HTML Toturials (PDF)",
                        Url = "https://some-url"
                    },
                    new Download
                    {
                        ModuleId = moduleId1, CourseId = courseId3, Title = "JavaScript Toturials (PDF)",
                        Url = "https://some-url"
                    },
                    new Download
                    {
                        ModuleId = moduleId1, CourseId = courseId3, Title = "HTML Toturials (PDF)",
                        Url = "https://some-url"
                    }
                };

                Downloads.AddRange(downloads);
                SaveChanges();
            }

            #endregion Add Downloads if they don't already exist
        }
    }
}