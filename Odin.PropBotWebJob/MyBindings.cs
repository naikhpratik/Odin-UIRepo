using AutoMapper;
using Ninject.Modules;
using Odin.Data.Core;
using Odin.Data.Persistence;
using Odin.PropBotWebJob.Domain;
using Odin.PropBotWebJob.Helpers;
using Odin.PropBotWebJob.Interfaces;

namespace Odin.PropBotWebJob
{
    public class MyBindings : NinjectModule
    {
        public override void Load()
        {
            Bind<IApplicationDbContext>().To<ApplicationDbContext>();
            Bind<IUnitOfWork>().To<UnitOfWork>();
            Bind<IImageStore>().To<ImageStore>();
            Bind<IBotHelper>().To<BotHelper>();

            var config = new MapperConfiguration(c => c.AddProfile(new MappingProfile()));
            var mapper = config.CreateMapper();
            Bind<IMapper>().ToConstant(mapper);
        }
    }
}
