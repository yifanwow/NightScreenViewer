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

    // 获取不透明度调整控件
    const opacitySlider = document.getElementById('opacity');
    const decreaseButton = document.getElementById('decrease');
    const increaseButton = document.getElementById('increase');

    if (isChecked) {
      // 禁用不透明度调整控件并将不透明度调整为100%     
      opacitySlider.value = 100;
      opacitySlider.oninput();
  
      opacitySlider.disabled = true;
      decreaseButton.classList.add('disabled');
      increaseButton.classList.add('disabled');
    } else {
      // 启用不透明度调整控件
      opacitySlider.disabled = false;
      decreaseButton.classList.remove('disabled');
      increaseButton.classList.remove('disabled');
    }
  });
});
