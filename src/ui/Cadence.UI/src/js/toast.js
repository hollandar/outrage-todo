export class Toaster extends HTMLElement {
    timer;
    toasts = [];

    get interval() {
        if (!this.hasAttribute("interval")) {
            return 500;
        }

        return this.getAttribute("interval") || 500;
    }

    set interval(val) {
        this.setAttribute("interval", val);
        clearInterval(this.timer);
        setInterval(this.tick, val);
    }
    constructor() {
        super()
        document.addEventListener("toast", this.toast);
        this.timer = setInterval(this.tick, this.interval);
        this.render();
    }

    render() {
        var toasts = this.toasts.map((t) => `<div class="toast ${t.type}">${t.message}</div>`);
        this.innerHTML = `<div class="toaster">${toasts.join("") }</div>` ;
    }

    tick = () => {
        for (let i = 0; i < this.toasts.length; i++) {
            if (this.toasts[i].duration > 0) {
                this.toasts[i].duration -= 500;
            } else {
                this.toasts.splice(i, 1);
            }
        }
        this.render();
    }

    toast = (e) => {
        this.toasts.push({ message: e.detail.message, type: e.detail.type, duration: e.detail.duration })
        this.render()
    }

    static publish(message, type, duration) {
        document.dispatchEvent(new CustomEvent("toast", { bubbles: true, composed: true, detail: new { message: message, type: type || "default", duration: duration || 4000 } }))
    }

}