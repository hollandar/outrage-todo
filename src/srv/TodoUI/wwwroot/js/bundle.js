class Toggler extends HTMLElement {
    constructor() {
        super();
        this.attachShadow({ mode: "open" });
        this.render();
    }

    get toggleEvent() {
        return this.getAttribute("toggle-event")
    }

    set toggleEvent(val) {
        this.setAttribute("toggle-event", val);
    }

    render = () => {
        this.shadowRoot.innerHTML = `
        <div class="toggler">
            <slot></slot>
        </div>
        `;
        this.shadowRoot.querySelector(".toggler").addEventListener("click", () => {
            this.dispatchToggledEvent();
        });
    }

    dispatchToggledEvent = () => {
        this.shadowRoot.dispatchEvent(new CustomEvent(this.toggleEvent, { bubbles: true, composed: true }));
    }
}

class Toggled extends HTMLElement {

    constructor() {
        super();
        document.addEventListener(this.toggleEvent, this.toggle);
        this.attachShadow({ mode: "open" });
        this.render();
    }

    static observedAttributes = ["toggled"];

    attributeChangedCallback(name, oldValue, newValue) {
        this.render();
    }

    get toggleEvent() {
        return this.getAttribute("toggle-event") || "toggle-event";
    }

    set toggleEvent(val) {
        document.removeEventListener(this.toggleEvent, this.toggle);
        this.setAttribute("toggle-event", val);
        document.addEventListener(this.toggleEvent, this.toggle);
    }

    get toggled() {
        return this.hasAttribute("toggled")
    }

    set toggled(val) {
        if (val) {
            this.setAttribute("toggled", "true");
        } else {
            this.removeAttribute("toggled");
        }
    }

    render = () => {
        if (this.toggled) {
            this.shadowRoot.innerHTML = `<slot></slot>`;
        } else {
            this.shadowRoot.innerHTML = `<slot style="display:none"></slot>`;
        }
    }

    toggle = (e) => {
        this.toggled = !this.toggled;
        this.render();
    }
}

class Toaster extends HTMLElement {
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
        super();
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
        this.toasts.push({ message: e.detail.message, type: e.detail.type, duration: e.detail.duration });
        this.render();
    }

    static publish(message, type, duration) {
        document.dispatchEvent(new CustomEvent("toast", { bubbles: true, composed: true, detail: new { message: message, type: type || "default", duration: duration || 4000 } }));
    }

}

customElements.define("ui-toggler", Toggler);
customElements.define("ui-toggled", Toggled);
customElements.define("ui-toaster", Toaster);
