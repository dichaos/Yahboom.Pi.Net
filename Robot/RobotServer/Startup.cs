﻿using System.Device.Gpio;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Robot.Configs;
using Robot.Devices;
using RobotServer.ServiceItems;
using RobotServer.Services;

namespace RobotServer
{
    public class Startup
    {
        private readonly RobotSettings config;

        public Startup(IConfiguration configuration)
        {
            var conf = configuration.Get<RobotServerCofig>();
            config = conf.RobotSettings;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();
            services.AddSingleton<RobotService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<RobotService>();

                endpoints.MapGet("/",
                    async context =>
                    {
                        await context.Response.WriteAsync(
                            "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                    });
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.Register(p => config.Buzzer).SingleInstance();
            builder.Register(p => config.AudioSettings).SingleInstance();
            builder.Register(p => config.CameraSettings).SingleInstance();
            builder.Register(p => config.MovementSettings).SingleInstance();
            builder.Register(p => config.TrackerSettings).SingleInstance();
            builder.Register(p => config.UltrasonicSettings).SingleInstance();
            builder.Register(p => config.LEDSettings).SingleInstance();

            builder.RegisterType<AudioServiceItem>()
                .As<IAudioServiceItem>()
                .SingleInstance();

            builder.RegisterType<BuzzServiceItem>()
                .As<IBuzzServiceItem>()
                .SingleInstance();

            builder.RegisterType<CameraServiceItem>()
                .As<ICameraServiceItem>()
                .SingleInstance();

            builder.RegisterType<MovementServiceItem>()
                .As<IMovementServiceItem>()
                .SingleInstance();

            builder.RegisterType<RGBServiceItem>()
                .As<IRGBServiceItem>()
                .SingleInstance();

            builder.RegisterType<TrackerServiceItem>()
                .As<ITrackerServiceItem>()
                .SingleInstance();

            builder.RegisterType<UltrasonicServiceItem>()
                .As<IUltrasonicServiceItem>()
                .SingleInstance();

            builder.RegisterType<GpioController>()
                .WithParameter(new TypedParameter(typeof(PinNumberingScheme), PinNumberingScheme.Logical))
                .SingleInstance();

            builder.RegisterType<Buzzer>()
                .As<IBuzzer>()
                .WithParameter(new TypedParameter(typeof(int), config.Buzzer))
                .SingleInstance();

            builder.RegisterType<LED>()
                .As<ILED>()
                .SingleInstance();

            builder.RegisterType<Microphone>()
                .As<IMicrophone>()
                .SingleInstance();

            builder.RegisterType<Camera>()
                .As<ICamera>()
                .SingleInstance();

            builder.RegisterType<Tracker>()
                .As<ITracker>()
                .SingleInstance();

            builder.RegisterType<Ultrasonic>()
                .As<IUltrasonic>()
                .SingleInstance();

            builder.RegisterType<Movement>()
                .As<IMovement>()
                .SingleInstance();
        }
    }
}