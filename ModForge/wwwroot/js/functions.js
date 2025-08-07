document.addEventListener('wheel', function (e) {
    if (e.ctrlKey) {
        e.preventDefault();
    }
}, { passive: false });

document.addEventListener('keydown', function (e) {
    if (e.ctrlKey && (e.key === '+' || e.key === '-' || e.key === '0')) {
        e.preventDefault();
    }
});
window.clipboardCopy = {
    copyText: function (text) {
        navigator.clipboard.writeText(text).then(function () {
        })
            .catch(function (error) {
                alert(error);
            });
    }
};
window.removeInitialLoader = function () {
    const loader = document.getElementById('initial-loader');
    if (loader) {
        loader.remove();
    }
};