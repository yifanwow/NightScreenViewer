document.addEventListener("DOMContentLoaded", () => {
  const app = document.getElementById("app");

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
        <button1 id="decrease" class="button1">
        <svg viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
        <g id="SVGRepo_bgCarrier" stroke-width="0"></g>
        <g id="SVGRepo_tracerCarrier" stroke-linecap="round" stroke-linejoin="round"></g>
        <g id="SVGRepo_iconCarrier"> <path d="M12 2C6.49 2 2 6.49 2 12C2 17.51 6.49 22 12 22C17.51 22 22 17.51 22 12C22 6.49 17.51 2 12 2ZM15.92 12.75H7.92C7.51 12.75 7.17 12.41 7.17 12C7.17 11.59 7.51 11.25 7.92 11.25H15.92C16.33 11.25 16.67 11.59 16.67 12C16.67 12.41 16.34 12.75 15.92 12.75Z" fill="currentColor"></path>
        </g>
        </svg>
        
        
        </button1>
        <input type="range" id="opacity" min="0" max="100" value="50">
        <button1 id="increase" class="button1">
        <svg viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
            <g id="SVGRepo_bgCarrier" stroke-width="0"></g>
            <g id="SVGRepo_tracerCarrier" stroke-linecap="round" stroke-linejoin="round"></g>
            <g id="SVGRepo_iconCarrier">
              <path fill-rule="evenodd" clip-rule="evenodd" d="M13 9C13 8.44772 12.5523 8 12 8C11.4477 8 11 8.44772 11 9V11H9C8.44772 11 8 11.4477 8 12C8 12.5523 8.44772 13 9 13H11V15C11 15.5523 11.4477 16 12 16C12.5523 16 13 15.5523 13 15V13H15C15.5523 13 16 12.5523 16 12C16 11.4477 15.5523 11 15 11H13V9ZM2 12C2 6.47715 6.47715 2 12 2C17.5228 2 22 6.47715 22 12C22 17.5228 17.5228 22 12 22C6.47715 22 2 17.5228 2 12Z" fill="currentColor"></path>
            </g>
          </svg>
        </button1>
      </div>
      <label for="opacity">Opacity: <span id="opacityValue">50%</span></label>
    </div>
  `;

  document.getElementById("autoOn").addEventListener("click", function () {
    const button = this;
    const isOn = button.classList.contains("button-on");
    if (isOn) {
      button.classList.remove("button-on");
      button.classList.add("button-off");
      button.textContent = "Auto Off";
    } else {
      button.classList.remove("button-off");
      button.classList.add("button-on");
      button.textContent = "Auto On";
    }
    window.electron.sendMessage(isOn ? "autoOn" : "autoOff");
  });

  const opacitySlider = document.getElementById("opacity");
  const opacityValue = document.getElementById("opacityValue");

  // 防抖函数
  function debounce(func, wait) {
    let timeout;
    return function (...args) {
      const context = this;
      clearTimeout(timeout);
      timeout = setTimeout(() => func.apply(context, args), wait);
    };
  }

  // 更新不透明度并发送消息的函数
  const updateOpacity = debounce((value) => {
    window.electron.sendMessage(`setOpacity:${value}`);
  }, 520); // 0.52秒钟防抖时间

  opacitySlider.oninput = function () {
    opacityValue.textContent = `${this.value}%`;
    updateOpacity(this.value); // 调用防抖函数
  };

  document.getElementById("decrease").addEventListener("click", () => {
    let newValue = parseInt(opacitySlider.value) - 5;
    if (newValue < 0) newValue = 0;
    opacitySlider.value = newValue;
    opacitySlider.oninput();
  });

  document.getElementById("increase").addEventListener("click", () => {
    let newValue = parseInt(opacitySlider.value) + 5;
    if (newValue > 100) newValue = 100;
    opacitySlider.value = newValue;
    opacitySlider.oninput();
  });

  document.getElementById("toggle").addEventListener("change", function () {
    const isChecked = this.checked;

    window.electron
      .sendMessage("autoOff")
      .then(() => {
        const autoOnButton = document.getElementById("autoOn");
        autoOnButton.classList.remove("button-off");
        autoOnButton.classList.add("button-on");
        autoOnButton.textContent = "Auto On";

        return window.electron.sendMessage(
          isChecked ? "startBlackScreen" : "stopBlackScreen"
        );
      })
      .then(() => {
        if (isChecked) {
          setTimeout(() => {
            const currentOpacity = opacitySlider.value;
            window.electron.sendMessage(`setOpacity:${currentOpacity}`);
          }, 1000); // 一秒钟延时
        }
      });
  });

  window.electron.receiveMessage((data) => {
    console.log("Message from REC:", data);
    const toggle = document.getElementById("toggle");
    if (data === "fullscreen.") {
      if (!toggle.checked) {//如果还没开启黑屏则开启，如果已经开启黑屏就忽略。
        window.electron.sendMessage("startBlackScreen");
        setTimeout(() => {
          const currentOpacity = opacitySlider.value;
          window.electron.sendMessage(`setOpacity:${currentOpacity}`);
        }, 1000); // 一秒钟延时
        toggle.checked = true;
      }
    } else if (data === "exitfullscreen.") {
      toggle.checked = false;
      window.electron.sendMessage("stopBlackScreen");
    }
  });
});
