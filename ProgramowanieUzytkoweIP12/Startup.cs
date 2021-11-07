using CQRS;
using CQRS.Authors;
using CQRS.Books;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Model;
using Model.DTO;
using RepositoryPattern;
using System;
using System.Collections.Generic;

namespace ProgramowanieUzytkoweIP12
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<Database>();

            services.AddScoped<LibraryRepository>();

            services.AddScoped<CommandBus>();
            services.AddScoped<QueryBus>();

            services.AddScoped<IQueryHandler<GetBooksQuery, List<BookDTO>>, GetBooksQueryHandler>();
            services.AddScoped<IQueryHandler<GetBookQuery, BookDTO>, GetBookQueryHandler>();
            services.AddScoped<ICommandHandler<AddBookCommand>, AddBookCommandHandler>();
            services.AddScoped<ICommandHandler<DeleteBookCommand>, DeleteBookCommandHandler>();
            services.AddScoped<ICommandHandler<AddBookRateCommand>, AddBookRateCommandHandler>();

            services.AddScoped<IQueryHandler<GetAuthorsQuery, List<AuthorDTO>>, GetAuthorsQueryHandler>();
            services.AddScoped<ICommandHandler<AddAuthorCommand>, AddAuthorCommandHandler>();
            services.AddScoped<ICommandHandler<DeleteAuthorCommand>, DeleteAuthorCommandHandler>();
            services.AddScoped<ICommandHandler<AddAuthorRateCommand>, AddAuthorRateCommandHandler>();

            services.AddSwaggerGen();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
