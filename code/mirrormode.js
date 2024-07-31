// mirrormode.js
document.addEventListener('DOMContentLoaded', () => {
  console.log('DOM content loaded');

  // 获取 checkbox 元素
  const mirrorModeCheckbox = document.getElementById('cb-47');

  // 添加事件监听器
  mirrorModeCheckbox.addEventListener('change', function () {
    const isChecked = this.checked;
    console.log("Mirror Mode checkbox changed, isChecked:", isChecked);

    // 发送消息给后端
    window.electron.sendMessage(isChecked ? 'mirrorModeOn' : 'mirrorModeOff');
  });
});
