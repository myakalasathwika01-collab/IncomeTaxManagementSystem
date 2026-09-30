/**
 * Enterprise Sidebar Controller for Income Tax Management System
 */
document.addEventListener('DOMContentLoaded', function () {
    const sidebarToggleBtn = document.getElementById('sidebarToggleBtn');
    const sidebarPinBtn = document.getElementById('sidebarPinBtn');
    const backdrop = document.getElementById('sidebarBackdrop');
    const body = document.body;

    // Restore desktop collapsed state from localStorage
    const isCollapsed = localStorage.getItem('sidebar_collapsed') === 'true';
    if (isCollapsed && window.innerWidth >= 992) {
        body.classList.add('sidebar-collapsed');
    }

    // Toggle Desktop Collapse
    function toggleDesktopCollapse() {
        body.classList.toggle('sidebar-collapsed');
        const collapsed = body.classList.contains('sidebar-collapsed');
        localStorage.setItem('sidebar_collapsed', collapsed ? 'true' : 'false');
        initTooltips();
    }

    // Toggle Mobile Offcanvas
    function toggleMobileSidebar() {
        body.classList.toggle('mobile-sidebar-open');
    }

    // Close Mobile Sidebar
    function closeMobileSidebar() {
        body.classList.remove('mobile-sidebar-open');
    }

    // Unified Toggle
    if (sidebarToggleBtn) {
        sidebarToggleBtn.addEventListener('click', function (e) {
            e.preventDefault();
            if (window.innerWidth < 992) {
                toggleMobileSidebar();
            } else {
                toggleDesktopCollapse();
            }
        });
    }

    if (sidebarPinBtn) {
        sidebarPinBtn.addEventListener('click', function (e) {
            e.preventDefault();
            toggleDesktopCollapse();
        });
    }

    if (backdrop) {
        backdrop.addEventListener('click', closeMobileSidebar);
    }

    // Close mobile sidebar on navigation link click
    const sidebarLinks = document.querySelectorAll('.sidebar-link');
    sidebarLinks.forEach(function (link) {
        link.addEventListener('click', function () {
            if (window.innerWidth < 992) {
                closeMobileSidebar();
            }
        });
    });

    // Initialize or destroy Bootstrap tooltips depending on collapsed state
    function initTooltips() {
        const isCollapsedNow = body.classList.contains('sidebar-collapsed');
        sidebarLinks.forEach(function (link) {
            const existingTooltip = bootstrap.Tooltip.getInstance(link);
            if (isCollapsedNow && window.innerWidth >= 992) {
                if (!existingTooltip) {
                    new bootstrap.Tooltip(link, {
                        placement: 'right',
                        trigger: 'hover',
                        boundary: 'window'
                    });
                }
            } else {
                if (existingTooltip) {
                    existingTooltip.dispose();
                }
            }
        });
    }

    // Window resize handler
    window.addEventListener('resize', function () {
        if (window.innerWidth >= 992) {
            closeMobileSidebar();
            const savedState = localStorage.getItem('sidebar_collapsed') === 'true';
            if (savedState) {
                body.classList.add('sidebar-collapsed');
            }
        } else {
            body.classList.remove('sidebar-collapsed');
        }
        initTooltips();
    });

    initTooltips();
});
