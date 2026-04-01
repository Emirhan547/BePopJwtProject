using BePopJwt.DataAccess.Context;
using BePopJwt.Entity.Entities;
using BePopJwt.Entity.Enums;
using Microsoft.EntityFrameworkCore;

namespace BePopJwt.DataAccess.Seed
{
    public static class AppDbSeeder
    {
        public static async Task SeedAsync(AppDbContext context, CancellationToken cancellationToken = default)
        {
            await context.Database.MigrateAsync(cancellationToken);

            if (!await context.Packages.AnyAsync(cancellationToken))
            {
                var packages = new List<Package>
                {
                    new() { Name = "Elite", Level = 1, Price = 299.99m },
                    new() { Name = "Premium", Level = 2, Price = 199.99m },
                    new() { Name = "Gold", Level = 3, Price = 129.99m },
                    new() { Name = "Standard", Level = 4, Price = 79.99m },
                    new() { Name = "Basic", Level = 5, Price = 39.99m },
                    new() { Name = "Free", Level = 6, Price = 0m }
                };

                await context.Packages.AddRangeAsync(packages, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
            }

            if (!await context.Artists.AnyAsync(cancellationToken))
            {
                var artists = new List<Artist>
                {
                    new() { Name = "Nova Echo", ImageUrl = "https://picsum.photos/seed/artist-nova/200/200", CoverUrl = "https://picsum.photos/seed/artist-nova-cover/1200/450", Bio = "Alternative pop soundscapes with cinematic vocals." },
                    new() { Name = "Lunar Isles", ImageUrl = "https://picsum.photos/seed/artist-lunar/200/200", CoverUrl = "https://picsum.photos/seed/artist-lunar-cover/1200/450", Bio = "Dream-pop duo blending analog synths and airy harmonies." },
                    new() { Name = "Mira Vale", ImageUrl = "https://picsum.photos/seed/artist-mira/200/200", CoverUrl = "https://picsum.photos/seed/artist-mira-cover/1200/450", Bio = "Neo-soul toplines and mellow electronic grooves." },
                    new() { Name = "Static Bloom", ImageUrl = "https://picsum.photos/seed/artist-static/200/200", CoverUrl = "https://picsum.photos/seed/artist-static-cover/1200/450", Bio = "Dance-pop collective focused on high-energy hooks." },
                    new() { Name = "Arda Kaan", ImageUrl = "https://picsum.photos/seed/artist-arda/200/200", CoverUrl = "https://picsum.photos/seed/artist-arda-cover/1200/450", Bio = "Anatolian motifs meeting modern electronic production." },
                    new() { Name = "Selin Rays", ImageUrl = "https://picsum.photos/seed/artist-selin/200/200", CoverUrl = "https://picsum.photos/seed/artist-selin-cover/1200/450", Bio = "Indie-pop storyteller with warm acoustic textures." }
                };

                await context.Artists.AddRangeAsync(artists, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
            }

            if (!await context.Albums.AnyAsync(cancellationToken))
            {
                var artistIds = await context.Artists
                    .AsNoTracking()
                    .ToDictionaryAsync(x => x.Name, x => x.Id, cancellationToken);

                var albums = new List<Album>
                {
                    new() { Name = "Afterglow Avenue", CoverUrl = "https://picsum.photos/seed/album-afterglow/400/400", ReleaseDate = new DateTime(2024, 3, 14), ArtistId = artistIds["Nova Echo"] },
                    new() { Name = "Polar Hearts", CoverUrl = "https://picsum.photos/seed/album-polar/400/400", ReleaseDate = new DateTime(2025, 1, 27), ArtistId = artistIds["Nova Echo"] },
                    new() { Name = "Moonwater", CoverUrl = "https://picsum.photos/seed/album-moonwater/400/400", ReleaseDate = new DateTime(2023, 11, 2), ArtistId = artistIds["Lunar Isles"] },
                    new() { Name = "Night Drive Diaries", CoverUrl = "https://picsum.photos/seed/album-nightdrive/400/400", ReleaseDate = new DateTime(2025, 2, 6), ArtistId = artistIds["Lunar Isles"] },
                    new() { Name = "Velvet Circuits", CoverUrl = "https://picsum.photos/seed/album-velvet/400/400", ReleaseDate = new DateTime(2024, 6, 21), ArtistId = artistIds["Mira Vale"] },
                    new() { Name = "Quiet Voltage", CoverUrl = "https://picsum.photos/seed/album-voltage/400/400", ReleaseDate = new DateTime(2025, 9, 12), ArtistId = artistIds["Mira Vale"] },
                    new() { Name = "Neon Horizon", CoverUrl = "https://picsum.photos/seed/album-neon/400/400", ReleaseDate = new DateTime(2024, 8, 30), ArtistId = artistIds["Static Bloom"] },
                    new() { Name = "Pulse Theory", CoverUrl = "https://picsum.photos/seed/album-pulse/400/400", ReleaseDate = new DateTime(2025, 4, 19), ArtistId = artistIds["Static Bloom"] },
                    new() { Name = "Anka", CoverUrl = "https://picsum.photos/seed/album-anka/400/400", ReleaseDate = new DateTime(2023, 4, 9), ArtistId = artistIds["Arda Kaan"] },
                    new() { Name = "Dalgalar", CoverUrl = "https://picsum.photos/seed/album-dalgalar/400/400", ReleaseDate = new DateTime(2025, 7, 11), ArtistId = artistIds["Arda Kaan"] },
                    new() { Name = "Amber Letters", CoverUrl = "https://picsum.photos/seed/album-amber/400/400", ReleaseDate = new DateTime(2024, 10, 13), ArtistId = artistIds["Selin Rays"] },
                    new() { Name = "Sunroom Stories", CoverUrl = "https://picsum.photos/seed/album-sunroom/400/400", ReleaseDate = new DateTime(2025, 12, 1), ArtistId = artistIds["Selin Rays"] }
                };

                await context.Albums.AddRangeAsync(albums, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
            }

            if (!await context.Songs.AnyAsync(cancellationToken))
            {
                var albumIds = await context.Albums
                    .AsNoTracking()
                    .ToDictionaryAsync(x => x.Name, x => x.Id, cancellationToken);

                var songs = new List<Song>
                {
                    new() { Name = "City of Satellites", Duration = 214, FilePath = "/media/songs/city-of-satellites.mp3", CoverUrl = "https://picsum.photos/seed/song-citysat/300/300", Level = PackageLevel.Free, AlbumId = albumIds["Afterglow Avenue"] },
                    new() { Name = "Glass Rain", Duration = 186, FilePath = "/media/songs/glass-rain.mp3", CoverUrl = "https://picsum.photos/seed/song-glassrain/300/300", Level = PackageLevel.Basic, AlbumId = albumIds["Afterglow Avenue"] },
                    new() { Name = "Silver Frequency", Duration = 201, FilePath = "/media/songs/silver-frequency.mp3", CoverUrl = "https://picsum.photos/seed/song-silverfreq/300/300", Level = PackageLevel.Standard, AlbumId = albumIds["Polar Hearts"] },
                    new() { Name = "Arctic Lights", Duration = 239, FilePath = "/media/songs/arctic-lights.mp3", CoverUrl = "https://picsum.photos/seed/song-arctic/300/300", Level = PackageLevel.Gold, AlbumId = albumIds["Polar Hearts"] },

                    new() { Name = "Low Tide", Duration = 175, FilePath = "/media/songs/low-tide.mp3", CoverUrl = "https://picsum.photos/seed/song-lowtide/300/300", Level = PackageLevel.Free, AlbumId = albumIds["Moonwater"] },
                    new() { Name = "Cloud Geometry", Duration = 223, FilePath = "/media/songs/cloud-geometry.mp3", CoverUrl = "https://picsum.photos/seed/song-cloudgeo/300/300", Level = PackageLevel.Standard, AlbumId = albumIds["Moonwater"] },
                    new() { Name = "Retrograde", Duration = 198, FilePath = "/media/songs/retrograde.mp3", CoverUrl = "https://picsum.photos/seed/song-retrograde/300/300", Level = PackageLevel.Basic, AlbumId = albumIds["Night Drive Diaries"] },
                    new() { Name = "Streetlight Mirage", Duration = 207, FilePath = "/media/songs/streetlight-mirage.mp3", CoverUrl = "https://picsum.photos/seed/song-mirage/300/300", Level = PackageLevel.Premium, AlbumId = albumIds["Night Drive Diaries"] },

                    new() { Name = "Soft Code", Duration = 191, FilePath = "/media/songs/soft-code.mp3", CoverUrl = "https://picsum.photos/seed/song-softcode/300/300", Level = PackageLevel.Free, AlbumId = albumIds["Velvet Circuits"] },
                    new() { Name = "Saffron Sky", Duration = 216, FilePath = "/media/songs/saffron-sky.mp3", CoverUrl = "https://picsum.photos/seed/song-saffron/300/300", Level = PackageLevel.Gold, AlbumId = albumIds["Velvet Circuits"] },
                    new() { Name = "Midnight Silk", Duration = 203, FilePath = "/media/songs/midnight-silk.mp3", CoverUrl = "https://picsum.photos/seed/song-midnightsilk/300/300", Level = PackageLevel.Standard, AlbumId = albumIds["Quiet Voltage"] },
                    new() { Name = "Electric Petals", Duration = 244, FilePath = "/media/songs/electric-petals.mp3", CoverUrl = "https://picsum.photos/seed/song-petals/300/300", Level = PackageLevel.Premium, AlbumId = albumIds["Quiet Voltage"] },

                    new() { Name = "Rooftop Motion", Duration = 205, FilePath = "/media/songs/rooftop-motion.mp3", CoverUrl = "https://picsum.photos/seed/song-rooftop/300/300", Level = PackageLevel.Free, AlbumId = albumIds["Neon Horizon"] },
                    new() { Name = "Laser Dust", Duration = 188, FilePath = "/media/songs/laser-dust.mp3", CoverUrl = "https://picsum.photos/seed/song-laserdust/300/300", Level = PackageLevel.Basic, AlbumId = albumIds["Neon Horizon"] },
                    new() { Name = "Heartbeat Protocol", Duration = 234, FilePath = "/media/songs/heartbeat-protocol.mp3", CoverUrl = "https://picsum.photos/seed/song-heartbeat/300/300", Level = PackageLevel.Gold, AlbumId = albumIds["Pulse Theory"] },
                    new() { Name = "Endless Feedback", Duration = 252, FilePath = "/media/songs/endless-feedback.mp3", CoverUrl = "https://picsum.photos/seed/song-feedback/300/300", Level = PackageLevel.Elite, AlbumId = albumIds["Pulse Theory"] },

                    new() { Name = "Kozmik Ruzgar", Duration = 193, FilePath = "/media/songs/kozmik-ruzgar.mp3", CoverUrl = "https://picsum.photos/seed/song-kozmik/300/300", Level = PackageLevel.Free, AlbumId = albumIds["Anka"] },
                    new() { Name = "Yildiz Tozu", Duration = 209, FilePath = "/media/songs/yildiz-tozu.mp3", CoverUrl = "https://picsum.photos/seed/song-yildiz/300/300", Level = PackageLevel.Standard, AlbumId = albumIds["Anka"] },
                    new() { Name = "Mavi Saat", Duration = 227, FilePath = "/media/songs/mavi-saat.mp3", CoverUrl = "https://picsum.photos/seed/song-mavi/300/300", Level = PackageLevel.Basic, AlbumId = albumIds["Dalgalar"] },
                    new() { Name = "Dalga Boyu", Duration = 236, FilePath = "/media/songs/dalga-boyu.mp3", CoverUrl = "https://picsum.photos/seed/song-dalgaboyu/300/300", Level = PackageLevel.Premium, AlbumId = albumIds["Dalgalar"] },

                    new() { Name = "Paper Boats", Duration = 179, FilePath = "/media/songs/paper-boats.mp3", CoverUrl = "https://picsum.photos/seed/song-paperboats/300/300", Level = PackageLevel.Free, AlbumId = albumIds["Amber Letters"] },
                    new() { Name = "Honeyed Words", Duration = 211, FilePath = "/media/songs/honeyed-words.mp3", CoverUrl = "https://picsum.photos/seed/song-honeyed/300/300", Level = PackageLevel.Basic, AlbumId = albumIds["Amber Letters"] },
                    new() { Name = "Windowlight", Duration = 197, FilePath = "/media/songs/windowlight.mp3", CoverUrl = "https://picsum.photos/seed/song-windowlight/300/300", Level = PackageLevel.Standard, AlbumId = albumIds["Sunroom Stories"] },
                    new() { Name = "Late Summer Tape", Duration = 241, FilePath = "/media/songs/late-summer-tape.mp3", CoverUrl = "https://picsum.photos/seed/song-latesummer/300/300", Level = PackageLevel.Gold, AlbumId = albumIds["Sunroom Stories"] }
                };

                await context.Songs.AddRangeAsync(songs, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
            }

            if (!await context.Banners.AnyAsync(cancellationToken))
            {
                var songs = await context.Songs
                    .AsNoTracking()
                    .OrderBy(x => x.Id)
                    .Take(6)
                    .ToListAsync(cancellationToken);

                var banners = songs.Select((song, index) => new Banner
                {
                    Title = $"Editor's Pick #{index + 1}",
                    Subtitle = $"{song.Name} • Haftanın favori seçkisi",
                    ImageUrl = $"https://picsum.photos/seed/banner-{index + 1}/1200/500",
                    SongId = song.Id
                }).ToList();

                await context.Banners.AddRangeAsync(banners, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}