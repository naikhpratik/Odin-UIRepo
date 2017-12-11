using AutoMapper;
using Odin.Data.Core;
using Odin.Data.Core.Dtos;
using Odin.Data.Core.Models;
using Odin.Interfaces;
using System;
using System.Web.Http;

namespace Odin.Controllers.Api
{
    public class BookMarkletController : ApiController
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IQueueStore _queueStore;
        private readonly IBookMarkletHelper _bookMarkletHelper;

        public BookMarkletController(IUnitOfWork unitOfWork, IMapper mapper, IQueueStore queueStore, IBookMarkletHelper bookMarkletHelper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _queueStore = queueStore;
            _bookMarkletHelper = bookMarkletHelper;
        }

        [HttpPost]
        [Authorize]
        public IHttpActionResult Add(BookMarkletDto dto)
        {

            if (String.IsNullOrEmpty(dto.OrderId) || String.IsNullOrEmpty(dto.PropertyUrl))
            {
                return BadRequest();
            }

            var queueEntry = _mapper.Map<BookMarkletDto, PropBotJobQueueEntry>(dto);
            _queueStore.Add(queueEntry);

            return Ok();
        }
    }
}
