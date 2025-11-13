// theatermode.js
document.addEventListener('DOMContentLoaded', () => {
  const theaterCheckbox = document.getElementById('cb-theater');

  if (!theaterCheckbox) return;

  theaterCheckbox.addEventListener('change', function () {
    const isChecked = this.checked;
    console.log("Theater Mode toggled:", isChecked);

    // 向后端发送命令
    window.electron.sendMessage(isChecked ? 'theaterOn' : 'theaterOff');
  });
});