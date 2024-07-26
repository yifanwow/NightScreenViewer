document.addEventListener('DOMContentLoaded', () => {
  const app = document.getElementById('app');

  app.innerHTML = `
    <div class="container">
      <h1>Night Screen Viewer</h1>
      <div class="toggle-container">
        <label class="switch">
          <input type="checkbox" id="toggle">
          <span class="slider round"></span>
        </label>
        <button id="autoOn" class="button button-on">Auto On</button>
      </div>
      <div class="input-container">
        <button id="decrease" class="button">-</button>
        <input type="range" id="opacity" min="0" max="100" value="50">
        <button id="increase" class="button">+</button>
      </div>
      <label for="opacity">Opacity: <span id="opacityValue">50%</span></label>
    </div>
  `;

  document.getElementById('autoOn').addEventListener('click', function() {
    const button = this;
    const isOn = button.classList.contains('button-on');
    if (isOn) {
      button.classList.remove('button-on');
      button.classList.add('button-off');
      button.textContent = 'Auto Off';
    } else {
      button.classList.remove('button-off');
      button.classList.add('button-on');
      button.textContent = 'Auto On';
    }
    window.electron.sendMessage(isOn ? 'autoOff' : 'autoOn');
  });

  const opacitySlider = document.getElementById('opacity');
  const opacityValue = document.getElementById('opacityValue');

  // 防抖函数
  function debounce(func, wait) {
    let timeout;
    return function(...args) {
      const context = this;
      clearTimeout(timeout);
      timeout = setTimeout(() => func.apply(context, args), wait);
    };
  }

  // 更新不透明度并发送消息的函数
  const updateOpacity = debounce((value) => {
    window.electron.sendMessage(`setOpacity:${value}`);
  }, 520); // 0.52秒钟防抖时间

  opacitySlider.oninput = function() {
    opacityValue.textContent = `${this.value}%`;
    updateOpacity(this.value); // 调用防抖函数
  };

  document.getElementById('decrease').addEventListener('click', () => {
    let newValue = parseInt(opacitySlider.value) - 5;
    if (newValue < 0) newValue = 0;
    opacitySlider.value = newValue;
    opacitySlider.oninput();
  });

  document.getElementById('increase').addEventListener('click', () => {
    let newValue = parseInt(opacitySlider.value) + 5;
    if (newValue > 100) newValue = 100;
    opacitySlider.value = newValue;
    opacitySlider.oninput();
  });

  document.getElementById('toggle').addEventListener('change', function() {
    const isChecked = this.checked;
    window.electron.sendMessage(isChecked ? 'startBlackScreen' : 'stopBlackScreen');

    // 当用户发送startBlackScreen后的一秒钟，同步发送一个更新不透明度的请求
    if (isChecked) {
      setTimeout(() => {
        const currentOpacity = opacitySlider.value;
        window.electron.sendMessage(`setOpacity:${currentOpacity}`);
      }, 1000); // 一秒钟延时
    }
  });

  window.electron.receiveMessage((data) => {
    console.log('Message from backend:', data);
  });
});
