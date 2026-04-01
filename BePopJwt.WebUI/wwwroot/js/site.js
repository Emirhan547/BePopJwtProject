// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
(function () {
    document.addEventListener('click', async (event) => {
        const playButton = event.target.closest('.chart-play-btn');
        if (playButton) {
            const player = document.getElementById('chart-player');
            const source = playButton.dataset.source;
            if (!player || !source) return;

            player.src = source;
            await player.play();
            return;
        }

        const restrictedButton = event.target.closest('.restricted-song-btn');
        if (restrictedButton) {
            const packagesUrl = restrictedButton.dataset.packagesUrl || '/Default/Packages';

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
