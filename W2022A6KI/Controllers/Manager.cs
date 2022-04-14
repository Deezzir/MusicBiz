// ************************************************************************************
// WEB524 Project Template V3 2221 == e284aa14-4d7d-4dc4-be29-2ffc878df7ff
// Do not change or remove the line above.
//
// By submitting this assignment you agree to the following statement.
// I declare that this assignment is my own work in accordance with the Seneca Academic
// Policy. No part of this assignment has been copied manually or electronically from
// any other source (including web sites) or distributed to other students.
// ************************************************************************************

using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using W2022A6KI.EntityModels;
using W2022A6KI.Models;

namespace W2022A6KI.Controllers
{
    public class Manager
    {
        // Reference to the data context
        private ApplicationDbContext ds = new ApplicationDbContext();

        // AutoMapper instance
        public IMapper mapper;

        // Request user property...

        // Backing field for the property
        private RequestUser _user;

        // Getter only, no setter
        public RequestUser User
        {
            get
            {
                // On first use, it will be null, so set its value
                if (_user == null)
                {
                    _user = new RequestUser(HttpContext.Current.User as ClaimsPrincipal);
                }
                return _user;
            }
        }

        // Default constructor...
        public Manager()
        {
            // Configure the AutoMapper components
            var config = new MapperConfiguration(cfg =>
            {

                cfg.CreateMap<RegisterViewModel, RegisterViewModelForm>();

                cfg.CreateMap<Genre, GenreBaseViewModel>();

                cfg.CreateMap<Track, TrackBaseViewModel>();
                cfg.CreateMap<Track, TrackWithDetailViewModel>();
                cfg.CreateMap<Track, TrackSampleBaseViewModel>();
                cfg.CreateMap<TrackAddViewModel, Track>();
                cfg.CreateMap<TrackBaseViewModel, TrackEditFormViewModel>();

                cfg.CreateMap<Album, AlbumBaseViewModel>();
                cfg.CreateMap<Album, AlbumWithDetailViewModel>();
                cfg.CreateMap<AlbumAddViewModel, Album>();

                cfg.CreateMap<Artist, ArtistBaseViewModel>();
                cfg.CreateMap<Artist, ArtistWithDetailViewModel>();
                cfg.CreateMap<ArtistAddViewModel, Artist>();

                cfg.CreateMap<ArtistMediaItem, ArtistMediaItemBaseViewModel>();
                cfg.CreateMap<ArtistMediaItem, ArtistMediaItemContentViewModel>();
                cfg.CreateMap<ArtistMediaItemAddViewModel, ArtistMediaItem>();
            });

            mapper = config.CreateMapper();

            // Turn off the Entity Framework (EF) proxy creation features
            // We do NOT want the EF to track changes - we'll do that ourselves
            ds.Configuration.ProxyCreationEnabled = false;

            // Also, turn off lazy loading...
            // We want to retain control over fetching related objects
            ds.Configuration.LazyLoadingEnabled = false;
        }

        #region ArtistMediaItem Managers
        public ArtistMediaItemContentViewModel ArtistMediaItemGetById(string stringId)
        {
            var mediaItem = ds.ArtistMediaItems.SingleOrDefault(it => it.StringId == stringId);
            return mediaItem != null ? mapper.Map<ArtistMediaItem, ArtistMediaItemContentViewModel>(mediaItem) : null;
        }

        public ArtistMediaItemBaseViewModel ArtistMediaItemAdd(ArtistMediaItemAddViewModel obj)
        {
            var artist = ds.Artists.Find(obj.ArtistId);
            if (artist == null) { return null; }

            var created = ds.ArtistMediaItems.Add(mapper.Map<ArtistMediaItemAddViewModel, ArtistMediaItem>(obj));
            if (created == null) { return null; }

            created.Artist = artist;

            byte[] itemBytes = new byte[obj.ItemUpload.ContentLength];
            obj.ItemUpload.InputStream.Read(itemBytes, 0, obj.ItemUpload.ContentLength);

            created.Content     = itemBytes;
            created.ContentType = obj.ItemUpload.ContentType;
            created.FileName    = obj.ItemUpload.FileName;

            ds.SaveChanges();
            return mapper.Map<ArtistMediaItem, ArtistMediaItemBaseViewModel>(created);
        }
        #endregion

        #region Genre Managers
        public IEnumerable<GenreBaseViewModel> GenreGetAll()
        {
            return mapper.Map<IEnumerable<Genre>, IEnumerable<GenreBaseViewModel>>(ds.Genres.OrderBy(g => g.Name));
        }
        #endregion

        #region Artist Managers
        public IEnumerable<ArtistBaseViewModel> ArtistGetAll()
        {
            return mapper.Map<IEnumerable<Artist>, IEnumerable<ArtistBaseViewModel>>(ds.Artists.OrderBy(a => a.Name));
        }

        public ArtistBaseViewModel ArtistGetById(int id)
        {
            var artist = ds.Artists.Find(id);
            return artist != null ? mapper.Map<Artist, ArtistBaseViewModel>(artist) : null;
        }

        public ArtistWithDetailViewModel ArtistGetByIdWithDetail(int id)
        {
            var artist = ds.Artists
                .Include("Albums")
                .Include("ArtistMediaItems")
                .SingleOrDefault(a => a.Id == id);
            return artist != null ? mapper.Map<Artist, ArtistWithDetailViewModel>(artist) : null;
        }

        public ArtistWithDetailViewModel ArtistAdd(ArtistAddViewModel obj)
        {
            obj.Executive = HttpContext.Current.User.Identity.Name;
            var created = ds.Artists.Add(mapper.Map<ArtistAddViewModel, Artist>(obj));

            if (created == null) { return null; }

            ds.SaveChanges();
            return mapper.Map<Artist, ArtistWithDetailViewModel>(created);
        }
        #endregion

        #region Album Managers
        public IEnumerable<AlbumBaseViewModel> AlbumGetAll()
        {
            return mapper.Map<IEnumerable<Album>, IEnumerable<AlbumBaseViewModel>>(ds.Albums.OrderBy(a => a.Name));
        }

        public AlbumBaseViewModel AlbumGetById(int id)
        {
            var album = ds.Albums.Find(id);
            return album != null ? mapper.Map<Album, AlbumBaseViewModel>(album) : null;
        }

        public AlbumWithDetailViewModel AlbumGetByIdWithDetail(int id)
        {
            var album = ds.Albums
                .Include("Tracks").Include("Artists")
                .SingleOrDefault(a => a.Id == id);
            return album != null ? mapper.Map<Album, AlbumWithDetailViewModel>(album) : null;
        }

        public AlbumWithDetailViewModel AlbumAdd(AlbumAddViewModel obj)
        {
            obj.Coordinator = HttpContext.Current.User.Identity.Name;
            var created = ds.Albums.Add(mapper.Map<AlbumAddViewModel, Album>(obj));

            if (created == null) { return null; }

            foreach (var trackId in obj.TrackIds)
            {
                var track = ds.Tracks.Find(trackId);
                created.Tracks.Add(track);
            }

            foreach (var artistId in obj.ArtistIds)
            {
                var artist = ds.Artists.Find(artistId);
                created.Artists.Add(artist);
            }

            ds.SaveChanges();
            return mapper.Map<Album, AlbumWithDetailViewModel>(created);
        }
        #endregion

        #region Tracks Managers
        public IEnumerable<TrackBaseViewModel> TrackGetAll()
        {
            return mapper.Map<IEnumerable<Track>, IEnumerable<TrackBaseViewModel>>(ds.Tracks.Include("Albums").OrderBy(a => a.Name));
        }

        public IEnumerable<TrackBaseViewModel> TrackGetAllByArtistId(int id)
        {
            var artist = ds.Artists
                .Include("Albums.Tracks").SingleOrDefault(a => a.Id == id);

            if (artist == null) { return null; }

            var tracks = new List<Track>();
            foreach (var album in artist.Albums)
            {
                tracks.AddRange(album.Tracks);
            }
            tracks = tracks.Distinct().ToList();

            return mapper.Map<IEnumerable<Track>, IEnumerable<TrackBaseViewModel>>(tracks.OrderBy(t => t.Name));
        }

        public TrackBaseViewModel TrackGetById(int id)
        {
            var track = ds.Tracks.Find(id);
            return track != null ? mapper.Map<Track, TrackBaseViewModel>(track) : null;
        }

        public TrackWithDetailViewModel TrackGetByIdWithDetail(int id)
        {
            var track = ds.Tracks
                .Include("Albums.Artists")
                .SingleOrDefault(t => t.Id == id);

            var artists = new List<Artist>();
            foreach(var album in track.Albums)
            {
                artists.AddRange(album.Artists);
            }
            artists = artists.Distinct().ToList();

            var details = track != null ? mapper.Map<Track, TrackWithDetailViewModel>(track) : null;
            if (details != null)
            {
                details.ArtistsCount = artists.Count;
                details.Artists = mapper.Map<IEnumerable<Artist>, IEnumerable<ArtistBaseViewModel>>(artists.OrderBy(t => t.Name));
            }

            return details;
        }

        public TrackSampleBaseViewModel TrackSampleGetById(int id)
        {
            var track = ds.Tracks.Find(id);
            return track != null ? mapper.Map<Track, TrackSampleBaseViewModel>(track) : null;
        }

        public TrackWithDetailViewModel TrackAdd(TrackAddViewModel obj)
        {
            obj.Clerk = HttpContext.Current.User.Identity.Name;

            var created = ds.Tracks.Add(mapper.Map<TrackAddViewModel, Track>(obj));
            if (created == null) { return null; }

            var album = ds.Albums.Find(obj.AlbumId);
            if (album == null) { return null; }

            created.Albums.Add(album);

            if (obj.SampleUpload != null)
            {
                byte[] sampleBytes = new byte[obj.SampleUpload.ContentLength];
                obj.SampleUpload.InputStream.Read(sampleBytes, 0, obj.SampleUpload.ContentLength);

                created.Sample = sampleBytes;
                created.SampleType = obj.SampleUpload.ContentType;
            }

            ds.SaveChanges();
            return mapper.Map<Track, TrackWithDetailViewModel>(created);
        }

        public TrackWithDetailViewModel TrackEdit(TrackEditViewModel obj)
        {
            var toEdit = ds.Tracks.Find(obj.Id);
            if (toEdit == null)
            {
                return null;
            }
            else
            {
                if (obj.SampleUpload != null)
                {
                    byte[] sampleBytes = new byte[obj.SampleUpload.ContentLength];
                    obj.SampleUpload.InputStream.Read(sampleBytes, 0, obj.SampleUpload.ContentLength);

                    toEdit.Sample = sampleBytes;
                    toEdit.SampleType = obj.SampleUpload.ContentType;

                    ds.SaveChanges();
                }

                return mapper.Map<Track, TrackWithDetailViewModel>(toEdit);
            }
        }

        public bool TrackDelete(int id)
        {
            var toDelete = ds.Tracks.Find(id);
            if(toDelete == null)
            {
                return false;
            }
            else
            {
                ds.Tracks.Remove(toDelete);
                ds.SaveChanges();
                return true;
            }
        }
        #endregion

        #region Role Claims

        public List<string> RoleClaimGetAllStrings()
        {
            return ds.RoleClaims.OrderBy(r => r.Name).Select(r => r.Name).ToList();
        }

        #endregion

        #region Load Data Methods

        // Add some programmatically-generated objects to the data store
        // You can write one method or many methods but remember to
        // check for existing data first.  You will call this/these method(s)
        // from a controller action.

        public bool LoadData()
        {
            // User name
            var user = HttpContext.Current.User.Identity.Name;

            // Monitor the progress
            bool success = true;

            // *** Role claims ***
            success &= RoleClaimsAdd();
            // *** Genres ***
            success &= GenresAdd();
            // *** Artists ***
            success &= ArtistsAdd();
            // *** Albums ***
            success &= AlbumsAdd();
            // *** Tracks ***
            success &= TracksAdd();

            return success;
        }

        public bool RoleClaimsAdd()
        {
            bool done = false;
            if (ds.RoleClaims.Count() == 0)
            {
                var executive = new RoleClaim { Name = "Coordinator" };
                var coordinator = new RoleClaim { Name = "Executive" };
                var clerk = new RoleClaim { Name = "Clerk" };
                var staff = new RoleClaim { Name = "Staff" };
                var admin = new RoleClaim { Name = "Admin " };

                ds.RoleClaims.Add(executive);
                ds.RoleClaims.Add(coordinator);
                ds.RoleClaims.Add(clerk);
                ds.RoleClaims.Add(staff);
                //ds.RoleClaims.Add(admin);

                ds.SaveChanges();
                done = true;
            }
            return done;
        }

        public bool GenresAdd()
        {
            bool done = false;
            if (ds.Genres.Count() == 0)
            {
                var g1 = new Genre { Name = "Pop" };
                var g2 = new Genre { Name = "Rock" };
                var g3 = new Genre { Name = "Hip-Hop & Rap" };
                var g4 = new Genre { Name = "Country" };
                var g5 = new Genre { Name = "R&B" };
                var g6 = new Genre { Name = "Folk" };
                var g7 = new Genre { Name = "Jazz" };
                var g8 = new Genre { Name = "Heavy Metal" };
                var g9 = new Genre { Name = "EDM" };
                var g10 = new Genre { Name = "Soul" };

                ds.Genres.Add(g1);
                ds.Genres.Add(g2);
                ds.Genres.Add(g3);
                ds.Genres.Add(g4);
                ds.Genres.Add(g5);
                ds.Genres.Add(g6);
                ds.Genres.Add(g7);
                ds.Genres.Add(g8);
                ds.Genres.Add(g9);
                ds.Genres.Add(g10);

                ds.SaveChanges();
                done = true;
            }
            return done;
        }

        public bool AlbumsAdd()
        {
            var user = "coord@example.com";
            bool done = false;
            if (ds.Albums.Count() == 0 && ds.Artists.Count() != 0)
            {
                var artist = ds.Artists.SingleOrDefault(a => a.Name == "AWOLNATION");

                var a1 = new Album
                {
                    Name = "Megalithic Symphony",
                    ReleaseDate = new DateTime(2011, 03, 15),
                    Genre = "Rock",
                    Coordinator = user,
                    UrlAlbum = "https://upload.wikimedia.org/wikipedia/en/d/da/Awolnation-Megalithic-Symphony.jpeg",
                    Artists = new List<Artist> { artist }
                };

                var a2 = new Album
                {
                    Name = "Run",
                    ReleaseDate = new DateTime(2015, 03, 17),
                    Genre = "Rock",
                    Coordinator = user,
                    UrlAlbum = "https://upload.wikimedia.org/wikipedia/en/thumb/c/cf/Awolnation_-_Run_%28album_cover%29.jpg/220px-Awolnation_-_Run_%28album_cover%29.jpg",
                    Artists = new List<Artist> { artist }
                };

                ds.Albums.Add(a1);
                ds.Albums.Add(a2);

                ds.SaveChanges();
                done = true;
            }
            return done;
        }

        public bool TracksAdd()
        {
            var user = "clerk@example.com";
            bool done = false;
            if (ds.Tracks.Count() == 0 && ds.Albums.Count() != 0)
            {
                var a1 = ds.Albums.SingleOrDefault(a => a.Name == "Megalithic Symphony");
                var a2 = ds.Albums.SingleOrDefault(a => a.Name == "Run");

                var t1 = new Track
                {
                    Name = "Soul Wars",
                    Genre = "Rock",
                    Clerk = user,
                    Composers = "Bruno, Eric Stenman",
                    Albums = new List<Album> { a1 }
                };

                var t2 = new Track
                {
                    Name = "People",
                    Genre = "Rock",
                    Clerk = user,
                    Composers = "Bruno, Eric Stenman",
                    Albums = new List<Album> { a1 }
                };

                var t3 = new Track
                {
                    Name = "Kill Your Heroes",
                    Genre = "Rock",
                    Clerk = user,
                    Composers = "Bruno, Brian West",
                    Albums = new List<Album> { a1 }
                };

                var t4 = new Track
                {
                    Name = "All I Need",
                    Genre = "Rock",
                    Clerk = user,
                    Composers = "Bruno, Messer",
                    Albums = new List<Album> { a1 }
                };

                var t5 = new Track
                {
                    Name = "Sail",
                    Genre = "Rock",
                    Clerk = user,
                    Composers = "Bruno, Messer",
                    Albums = new List<Album> { a1 }
                };

                var t6 = new Track
                {
                    Name = "Run",
                    Genre = "Rock",
                    Clerk = user,
                    Composers = "Bruno, Messer",
                    Albums = new List<Album> { a2 }
                };

                var t7 = new Track
                {
                    Name = "Jailbreak",
                    Genre = "Rock",
                    Clerk = user,
                    Composers = "Bruno",
                    Albums = new List<Album> { a2 }
                };

                var t8 = new Track
                {
                    Name = "Dreamers",
                    Genre = "Rock",
                    Clerk = user,
                    Composers = "Bruno",
                    Albums = new List<Album> { a2 }
                };

                var t9 = new Track
                {
                    Name = "Windows",
                    Genre = "Rock",
                    Clerk = user,
                    Composers = "Bruno, Messer",
                    Albums = new List<Album> { a2 }
                };

                var t10 = new Track
                {
                    Name = "Kookseverywhere!!!",
                    Genre = "Rock",
                    Clerk = user,
                    Composers = "Bruno",
                    Albums = new List<Album> { a2 }
                };

                ds.Tracks.Add(t1);
                ds.Tracks.Add(t2);
                ds.Tracks.Add(t3);
                ds.Tracks.Add(t4);
                ds.Tracks.Add(t5);
                ds.Tracks.Add(t6);
                ds.Tracks.Add(t7);
                ds.Tracks.Add(t8);
                ds.Tracks.Add(t9);
                ds.Tracks.Add(t10);

                ds.SaveChanges();
                done = true;
            }
            return done;
        }

        public bool ArtistsAdd()
        {
            var user = "exec@example.com";

            bool done = false;
            if (ds.Artists.Count() == 0)
            {
                var a1 = new Artist
                {
                    Name = "AWOLNATION",
                    BirthName = "Aaron Bruno",
                    BirthOrStartDate = new DateTime(2010, 05, 18),
                    Genre = "Rock",
                    Executive = user,
                    UrlArtist = "https://upload.wikimedia.org/wikipedia/commons/thumb/9/94/DJ_LAM_with_Awolnation.jpg/1024px-DJ_LAM_with_Awolnation.jpg"
                };

                var a2 = new Artist
                {
                    Name = "Valentin Strykalo",
                    BirthName = "Yury Kaplan",
                    BirthOrStartDate = new DateTime(1988, 10, 2),
                    Genre = "Rock",
                    Executive = user,
                    UrlArtist = "https://scontent.fykz1-1.fna.fbcdn.net/v/t1.6435-1/123717483_104601004796009_5661898660946434940_n.jpg?stp=dst-jpg_p720x720&_nc_cat=108&ccb=1-5&_nc_sid=1eb0c7&_nc_ohc=KNuWBq8t-34AX8n0cmZ&_nc_ht=scontent.fykz1-1.fna&oh=00_AT_s0gDRDHSr_NbWEN0mUG-D2u_p_ve_qJ5AB4S3bY-uVA&oe=6259858F"
                };

                var a3 = new Artist
                {
                    Name = "Rick Astley",
                    BirthName = "Richard Paul Astley",
                    BirthOrStartDate = new DateTime(1966, 02, 6),
                    Genre = "Pop",
                    Executive = user,
                    UrlArtist = "https://www.aboutmanchester.co.uk/wp-content/uploads/2020/04/DFA6B3F3-EBD2-48C4-A4C7-9BB23067CE01.jpeg"
                };

                ds.Artists.Add(a1);
                ds.Artists.Add(a2);
                ds.Artists.Add(a3);

                ds.SaveChanges();
                done = true;
            }
            return done;
        }

        public bool RemoveData()
        {
            try
            {
                foreach (var e in ds.RoleClaims)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                ds.SaveChanges();

                foreach (var e in ds.Genres)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                ds.SaveChanges();

                foreach (var e in ds.Albums)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                ds.SaveChanges();

                foreach (var e in ds.ArtistMediaItems)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                ds.SaveChanges();

                foreach (var e in ds.Artists)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                ds.SaveChanges();

                foreach (var e in ds.Tracks)
                {
                    ds.Entry(e).State = System.Data.Entity.EntityState.Deleted;
                }
                ds.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RemoveDatabase()
        {
            try
            {
                return ds.Database.Delete();
            }
            catch (Exception)
            {
                return false;
            }
        }

    }

    #endregion

    #region RequestUser Class

    // This "RequestUser" class includes many convenient members that make it
    // easier work with the authenticated user and render user account info.
    // Study the properties and methods, and think about how you could use this class.

    // How to use...
    // In the Manager class, declare a new property named User:
    //    public RequestUser User { get; private set; }

    // Then in the constructor of the Manager class, initialize its value:
    //    User = new RequestUser(HttpContext.Current.User as ClaimsPrincipal);

    public class RequestUser
    {
        // Constructor, pass in the security principal
        public RequestUser(ClaimsPrincipal user)
        {
            if (HttpContext.Current.Request.IsAuthenticated)
            {
                Principal = user;

                // Extract the role claims
                RoleClaims = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);

                // User name
                Name = user.Identity.Name;

                // Extract the given name(s); if null or empty, then set an initial value
                string gn = user.Claims.SingleOrDefault(c => c.Type == ClaimTypes.GivenName).Value;
                if (string.IsNullOrEmpty(gn)) { gn = "(empty given name)"; }
                GivenName = gn;

                // Extract the surname; if null or empty, then set an initial value
                string sn = user.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Surname).Value;
                if (string.IsNullOrEmpty(sn)) { sn = "(empty surname)"; }
                Surname = sn;

                IsAuthenticated = true;
                // You can change the string value in your app to match your app domain logic
                IsAdmin = user.HasClaim(ClaimTypes.Role, "Admin") ? true : false;
            }
            else
            {
                RoleClaims = new List<string>();
                Name = "anonymous";
                GivenName = "Unauthenticated";
                Surname = "Anonymous";
                IsAuthenticated = false;
                IsAdmin = false;
            }

            // Compose the nicely-formatted full names
            NamesFirstLast = $"{GivenName} {Surname}";
            NamesLastFirst = $"{Surname}, {GivenName}";
        }

        // Public properties
        public ClaimsPrincipal Principal { get; private set; }
        public IEnumerable<string> RoleClaims { get; private set; }

        public string Name { get; set; }

        public string GivenName { get; private set; }
        public string Surname { get; private set; }

        public string NamesFirstLast { get; private set; }
        public string NamesLastFirst { get; private set; }

        public bool IsAuthenticated { get; private set; }

        public bool IsAdmin { get; private set; }

        public bool HasRoleClaim(string value)
        {
            if (!IsAuthenticated) { return false; }
            return Principal.HasClaim(ClaimTypes.Role, value) ? true : false;
        }

        public bool HasClaim(string type, string value)
        {
            if (!IsAuthenticated) { return false; }
            return Principal.HasClaim(type, value) ? true : false;
        }
    }

    #endregion

}