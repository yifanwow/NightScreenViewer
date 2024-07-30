document.addEventListener("DOMContentLoaded", () => {
  console.log("DOM content loaded");

  // 等待 index_html_template.js 完成 DOM 操作
  setTimeout(() => {
    document.getElementById("autoOn").addEventListener("click", function () {
      const button = this;
      const isOn = button.classList.contains("button-on");
      console.log("AutoOn button clicked, isOn:", isOn);

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
    console.log("Opacity slider and value elements:", opacitySlider, opacityValue);

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
      console.log(`Updating opacity to ${value}`);
      window.electron.sendMessage(`setOpacity:${value}`);
    }, 520); // 0.52秒钟防抖时间

    function updateRangeBackground(value) {
      const percentage = value + '%';
      opacitySlider.style.background = `linear-gradient(to right, #5d4164 ${percentage}, #b9b9b9 ${percentage})`;
      console.log(`Range background updated to ${percentage}`);
    }

    opacitySlider.oninput = function () {
      opacityValue.textContent = `${this.value}%`;
      updateOpacity(this.value); // 调用防抖函数
      updateRangeBackground(this.value);
      console.log(`Opacity slider input changed to ${this.value}%`);
    };

    document.getElementById("decrease").addEventListener("click", () => {
      let newValue = parseInt(opacitySlider.value) - 5;
      if (newValue < 0) newValue = 0;
      opacitySlider.value = newValue;
      opacitySlider.oninput();
      console.log("Decrease button clicked, new value:", newValue);
    });

    document.getElementById("increase").addEventListener("click", () => {
      let newValue = parseInt(opacitySlider.value) + 5;
      if (newValue > 100) newValue = 100;
      opacitySlider.value = newValue;
      opacitySlider.oninput();
      console.log("Increase button clicked, new value:", newValue);
    });

    document.getElementById("toggle").addEventListener("change", function () {
      const isChecked = this.checked;
      console.log("Toggle changed, isChecked:", isChecked);

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
              console.log(`Black screen started with opacity ${currentOpacity}`);
            }, 1000); // 一秒钟延时
          }
        });
    });

    window.electron.receiveMessage((data) => {
      console.log("Message from REC:", data);
      const toggle = document.getElementById("toggle");
      if (data === "fullscreen.") {
        if (!toggle.checked) {
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

    // 初始化滑块背景
    updateRangeBackground(opacitySlider.value);
  }, 0); // 确保在 DOMContentLoaded 之后立即执行
});
