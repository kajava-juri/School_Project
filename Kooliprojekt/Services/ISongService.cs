using KooliProjekt.Data;
using KooliProjekt.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace KooliProjekt.Services
{
    public interface ISongService 
    {
        Task<PagedResult<SongListModel>> List(int page);
        Task<SongListModel> GetForDetail(int id);
        Task<SongCreationModel> GetForCreate();
        Task FillCreate(SongCreationModel model);
        Task<OperationResponse> Create(SongCreationModel model);
        Task<SongEditModel> GetForEdit(int id);
        Task Edit(SongEditModel model);
        Task<Song> GetForDelete(int id);
        Task Delete(int id);
        Task<PagedResult<SongDto>> ApiGetList();
    }
}
