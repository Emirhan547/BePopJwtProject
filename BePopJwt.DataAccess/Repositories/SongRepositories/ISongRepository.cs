using BePopJwt.DataAccess.Repositories.GenericRepositories;
using BePopJwt.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.DataAccess.Repositories.SongRepositories
{
    public interface ISongRepository:IRepository<Song>
    {
        Task<List<Song>> GetSongsWithAlbumAsync();
        Task<Song?> GetSongWithAlbumByIdAsync(int id);
    }
}
