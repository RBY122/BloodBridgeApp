document.addEventListener("DOMContentLoaded", function () {
  // ----------------------------
  // 1. Initialize Bootstrap Dropdowns
  // ----------------------------
  document.querySelectorAll(".dropdown-toggle").forEach(trigger => {
    new bootstrap.Dropdown(trigger);
  });

  // ----------------------------
  // 2. Improve FAQ Toggle with Dynamic Text Updates
  // ----------------------------
  document.addEventListener("click", event => {
    const button = event.target.closest(".card button");
    if (!button) return;
    const collapseElement = button.closest(".card")?.querySelector(".collapse");
    if (collapseElement) {
      collapseElement.addEventListener("shown.bs.collapse", () => {
        button.textContent = "🔽 Collapse Answer";
      });
      collapseElement.addEventListener("hidden.bs.collapse", () => {
        button.textContent = "▶ Expand Answer";
      });
    }
  });

  // ----------------------------
  // 3. Maintain Footer Visibility
  // ----------------------------
  document.body.style.paddingBottom = "80px";

  // ----------------------------
  // 4. Password Visibility Toggle & Validation
  // ----------------------------
  const passwordInput = document.getElementById("password");
  const togglePasswordButton = document.getElementById("toggle-password");
  if (passwordInput && togglePasswordButton) {
    togglePasswordButton.addEventListener("click", () => {
      const isPasswordHidden = passwordInput.type === "password";
      passwordInput.type = isPasswordHidden ? "text" : "password";
      togglePasswordButton.textContent = isPasswordHidden ? "Hide password" : "Show password";
      // Update accessibility label accordingly
      togglePasswordButton.setAttribute(
        "aria-label",
        isPasswordHidden
          ? "Hide password."
          : "Show password as plain text. Warning: this will display your password on the screen."
      );
    });
    // Clear custom validity when the input changes
    passwordInput.addEventListener("input", () => passwordInput.setCustomValidity(""));
  }
  function validatePassword() {
    if (!passwordInput) return;
    let validationMessages = [];
    const passwordValue = passwordInput.value;
    if (passwordValue.length < 8) validationMessages.push("At least eight characters.");
    if (!/[A-Z]/.test(passwordValue)) validationMessages.push("At least one uppercase letter.");
    if (!/[a-z]/.test(passwordValue)) validationMessages.push("At least one lowercase letter.");
    if (!/\d/.test(passwordValue)) validationMessages.push("At least one number.");
    if (!/[@$!%*?&]/.test(passwordValue)) validationMessages.push("At least one special character (@, $, !, etc.).");
    passwordInput.setCustomValidity(validationMessages.join(" "));
  }
  const form = document.querySelector("form");
  const signinButton = document.querySelector("button#sign-in");
  if (form && signinButton) {
    form.addEventListener("submit", event => {
      event.preventDefault();
      validatePassword();
      form.reportValidity();
      if (form.checkValidity()) {
        alert("Logging in!");
        signinButton.disabled = true; // Prevent multiple submissions
      }
    });
  }

  // ----------------------------
  // 5. Navigation Active Link Highlighting
  // ----------------------------
  (function highlightNavLink() {
    const currentPage = window.location.pathname.split("/").pop();
    document.querySelectorAll(".nav-item a").forEach(link => {
      if (link.href.includes(currentPage)) {
        link.parentElement.classList.add("active");
      }
    });
  })();

  // ----------------------------
  // 6. Payment Option Handling & Step Transition (jQuery)
  // ----------------------------
  $(function () {
    $(".payment-option").click(function () {
      $(".payment-option").removeClass("active");
      $(this).addClass("active");
    });
    $("#continueBtn").click(function () {
      let amount = $("#amount").val();
      let paymentMethodSelected = $(".payment-option.active").length > 0;
      $("#errorMessage").hide();
      if (!amount || amount <= 0) {
        showError("Please enter a valid donation amount.");
      } else if (!paymentMethodSelected) {
        showError("Please select a payment method before proceeding.");
      } else {
        $("#step1").hide();
        $("#step2").fadeIn();
      }
    });
    function showError(message) {
      $("#errorMessage").text(message).fadeIn();
    }
    $("#paymentSuccessBtn").click(function () {
      $("#step2").hide();
      $("#step3").fadeIn();
    });
  });

  // ----------------------------
  // 7. Initialize Team Carousel
  // ----------------------------
  const teamCarousel = document.querySelector('#teamCarousel');
  if (teamCarousel) {
    new bootstrap.Carousel(teamCarousel);
  }

  // ----------------------------
  // 8. Initialize Select2 and Multi-step Form Navigation (jQuery)
  // ----------------------------
  $(function () {
    $('.select2').select2();
    $('.next-step').click(function () {
      var currentStep = $(this).closest('.form-step');
      var nextStep = currentStep.next('.form-step');
      currentStep.hide();
      nextStep.show();
    });
    $('.prev-step').click(function () {
      var currentStep = $(this).closest('.form-step');
      var prevStep = currentStep.prev('.form-step');
      currentStep.hide();
      prevStep.show();
    });
  });

  // ----------------------------
  // 9. Multi-step Form Progress Bar & Navigation (Vanilla JS)
  // ----------------------------
  (function setupProgressAndNav() {
    const progressBar = document.querySelector("#completionBar");
    const completionText = document.querySelector("#profileCompletion");
    const profileFields = document.querySelectorAll("input, select, textarea");
    const steps = document.querySelectorAll(".form-step");
    let currentStep = 0;
  
    function showStep(stepIndex) {
      steps.forEach((step, index) => {
        step.style.display = index === stepIndex ? "block" : "none";
      });
    }
  
    document.querySelectorAll(".next-step").forEach(button => {
      button.addEventListener("click", () => {
        if (currentStep < steps.length - 1) {
          currentStep++;
          showStep(currentStep);
        }
      });
    });
    document.querySelectorAll(".prev-step").forEach(button => {
      button.addEventListener("click", () => {
        if (currentStep > 0) {
          currentStep--;
          showStep(currentStep);
        }
      });
    });
  
    function updateProgress() {
      let completedFields = Array.from(profileFields).filter(field => {
        return field.type === "checkbox" ? field.checked : field.value.trim() !== "";
      }).length;
      let percentage = (completedFields / profileFields.length) * 100;
      if (progressBar) progressBar.style.width = percentage + "%";
      if (progressBar) progressBar.setAttribute("aria-valuenow", percentage);
      if (completionText) completionText.innerText = Math.round(percentage) + "%";
    }
  
    profileFields.forEach(field => {
      field.addEventListener("input", updateProgress);
      field.addEventListener("change", updateProgress);
    });
  
    updateProgress(); // Initialize progress tracking
    showStep(currentStep); // Display the first step
  })();

  // ----------------------------
  // 10. Persistent Dark Mode Toggle with localStorage
  // ----------------------------
  const themeToggle = document.getElementById("theme-toggle");
  if (localStorage.getItem("theme") === "dark") {
    document.body.classList.add("dark-mode");
  }
  if (themeToggle) {
    themeToggle.addEventListener("click", () => {
      document.body.classList.toggle("dark-mode");
      localStorage.setItem("theme", document.body.classList.contains("dark-mode") ? "dark" : "light");
    });
  }

  // ----------------------------
  // 11. View More / View Less Toggle for Additional Details
  // ----------------------------
  var collapseElement = document.getElementById("moreDetails");
  var toggleDetailsBtn = document.getElementById("toggleDetailsBtn");
  if (collapseElement && toggleDetailsBtn) {
    collapseElement.addEventListener("shown.bs.collapse", function() {
      toggleDetailsBtn.textContent = "Show Less";
    });
    collapseElement.addEventListener("hidden.bs.collapse", function() {
      toggleDetailsBtn.textContent = "View More Details";
    });
  }


function toggleAccordion(header) {
  const item = header.parentElement;
  item.classList.toggle("open");
}

document.addEventListener("DOMContentLoaded", () => {
  feather.replace();

  // Counter Animation
  document.querySelectorAll('.counter').forEach(counter => {
    const target = +counter.getAttribute('data-target');
    let count = 0;

    const update = () => {
      count += Math.ceil(target / 200);
      if (count < target) {
        counter.innerText = count;
        requestAnimationFrame(update);
      } else {
        counter.innerText = target;
      }
    };

    update();
  });
});



  // ----------------------------
  // 12. Ensure Dropdown Closes Correctly After Selection on Mobile
  // ----------------------------
  document.addEventListener("click", event => {
    document.querySelectorAll(".dropdown-menu").forEach(dropdown => {
      if (!dropdown.contains(event.target)) {
        dropdown.style.visibility = "hidden";
        dropdown.style.opacity = "0";
      }
    });
  });
});
