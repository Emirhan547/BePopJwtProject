// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
(function () {
    async function getSongSource(songId) {
        const response = await fetch(`/Discovery/SongSource?songId=${songId}`);
        const data = await response.json();
        if (!response.ok || !data.ok || !data.source) {
            return null;
        }

        return data.source;
    }

    async function playOnPageIfAvailable(source) {
        const pagePlayer = document.querySelector('#web-player, #chart-player, #artist-player, #detail-player');
        if (!pagePlayer) return;
        pagePlayer.src = source;
        await pagePlayer.play();
    }
    document.addEventListener('click', async (event) => {
        const songLink = event.target.closest('.js-song-link');
        if (songLink) {
            event.preventDefault();
            const songId = Number(songLink.dataset.songId || 0);
            const href = songLink.getAttribute('href');
            if (!songId || !href) return;

            const source = await getSongSource(songId);
            if (source) {
                await playOnPageIfAvailable(source);
            }

            window.location.href = href;
            return;
        }

        const playButton = event.target.closest('.chart-play-btn, .artist-play-btn');
        if (playButton) {
            const songId = Number(playButton.dataset.songId || 0);
            if (!songId) return;

            const source = await getSongSource(songId);
            if (!source) return;
            await playOnPageIfAvailable(source);
            return;
        }

        const restrictedButton = event.target.closest('.restricted-song-btn');
        if (restrictedButton) {
            const packagesUrl = restrictedButton.dataset.packagesUrl || '/Package/Packages';

            if (window.Swal) {
                const result = await window.Swal.fire({
                    icon: 'warning',
                    title: 'Bu şarkıya erişim yok',
                    text: 'Bu müziği dinlemek için paketinizi yükseltmelisiniz.',
                    confirmButtonText: 'Paketlere Git',
                    showCancelButton: true,
                    cancelButtonText: 'Vazgeç'
                });

                if (result.isConfirmed) {
                    window.location.href = packagesUrl;
                }

                return;
            }

            if (window.confirm('Bu müzik için paket yükseltmeniz gerekiyor. Paketlere gidelim mi?')) {
                window.location.href = packagesUrl;
            }
        }
    });
})();
// Write your JavaScript code.
