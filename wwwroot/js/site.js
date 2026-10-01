(() => {
    const themeKey = "fcvt-theme";
    const root = document.documentElement;

    const updateCharts = () => {
        if (!window.Chart?.instances) return;

        const styles = getComputedStyle(root);
        const textColor = styles.getPropertyValue("--bs-body-color").trim();
        const gridColor = root.dataset.bsTheme === "dark" ? "#33445f" : "#dee2e6";

        Object.values(window.Chart.instances).forEach(chart => {
            chart.options.color = textColor;
            const legend = chart.options.plugins?.legend;
            if (legend) legend.labels = { ...legend.labels, color: textColor };
            Object.values(chart.options.scales ?? {}).forEach(scale => {
                scale.ticks = { ...scale.ticks, color: textColor };
                scale.grid = { ...scale.grid, color: gridColor };
            });
            chart.update();
        });
    };

    const applyTheme = (theme, announce) => {
        root.dataset.bsTheme = theme;
        localStorage.setItem(themeKey, theme);

        const nextTheme = theme === "dark" ? "claro" : "oscuro";
        document.querySelector("#theme-label")?.replaceChildren(`Modo ${nextTheme}`);
        document.querySelector("#theme-toggle")?.setAttribute("aria-label", `Cambiar a modo ${nextTheme}`);
        if (announce) document.querySelector("#theme-announcement").textContent = `Modo ${theme === "dark" ? "oscuro" : "claro"} activado`;
        updateCharts();
    };

    document.addEventListener("DOMContentLoaded", () => {
        applyTheme(root.dataset.bsTheme, false);
        document.querySelector("#theme-toggle")?.addEventListener("click", () =>
            applyTheme(root.dataset.bsTheme === "dark" ? "light" : "dark", true));
    });
})();
