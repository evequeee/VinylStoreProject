using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectVinylStore.DataAccess.Entities
{
    public class Album
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }

        public string Genre { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int AlbumId { get; set; }
    }
}
