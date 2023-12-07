export class Toggler extends HTMLElement {
    constructor() {
        super();
        this.attachShadow({ mode: "open" })
        this.render()
    }

    get toggleEvent() {
        return this.getAttribute("toggle-event")
    }

    set toggleEvent(val) {
        this.setAttribute("toggle-event", val)
    }

    render = () => {
        this.shadowRoot.innerHTML = `
        <div class="toggler">
            <slot></slot>
        </div>
        `;
        this.shadowRoot.querySelector(".toggler").addEventListener("click", () => {
            this.dispatchToggledEvent()
        })
    }

    dispatchToggledEvent = () => {
        this.shadowRoot.dispatchEvent(new CustomEvent(this.toggleEvent, { bubbles: true, composed: true }))
    }
}

export class Toggled extends HTMLElement {

    constructor() {
        super();
        document.addEventListener(this.toggleEvent, this.toggle);
        this.attachShadow({ mode: "open" })
        this.render()
    }

    static observedAttributes = ["toggled"];

    attributeChangedCallback(name, oldValue, newValue) {
        this.render()
    }

    get toggleEvent() {
        return this.getAttribute("toggle-event") || "toggle-event";
    }

    set toggleEvent(val) {
        document.removeEventListener(this.toggleEvent, this.toggle)
        this.setAttribute("toggle-event", val)
        document.addEventListener(this.toggleEvent, this.toggle);
    }

    get toggled() {
        return this.hasAttribute("toggled")
    }

    set toggled(val) {
        if (val) {
            this.setAttribute("toggled", "true")
        } else {
            this.removeAttribute("toggled")
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