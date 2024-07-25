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

  opacitySlider.oninput = function() {
    opacityValue.textContent = `${this.value}%`;
    window.electron.sendMessage(`setOpacity:${this.value}`);
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
  });

  window.electron.receiveMessage((data) => {
    console.log('Message from backend:', data);
  });
});
