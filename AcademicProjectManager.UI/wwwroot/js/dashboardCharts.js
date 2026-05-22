window.dashboardCharts = (function () {
    let completionChart;

    function renderMemberCompletionChart(canvasId, labels, dataPoints) {
        const canvas = document.getElementById(canvasId);
        if (!canvas || !window.Chart) {
            return;
        }

        if (completionChart) {
            completionChart.destroy();
        }

        completionChart = new Chart(canvas, {
            type: "bar",
            data: {
                labels,
                datasets: [
                    {
                        label: "Completion Rate (%)",
                        data: dataPoints,
                        borderRadius: 8,
                        backgroundColor: [
                            "#12616b",
                            "#1f7f91",
                            "#2e99a6",
                            "#44b3b9",
                            "#6bc7c6"
                        ]
                    }
                ]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        display: false
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true,
                        max: 100
                    }
                }
            }
        });
    }

    return {
        renderMemberCompletionChart
    };
})();
