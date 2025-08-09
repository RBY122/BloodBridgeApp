// 📦 Firebase Imports
import { initializeApp } from "firebase/app";
import { getAuth, onAuthStateChanged } from "firebase/auth";
import { getFirestore, doc, getDoc } from "firebase/firestore";
import { getAnalytics } from "firebase/analytics";

// 🚀 Firebase Configuration
const firebaseConfig = {
  apiKey: "AIzaSyCkxDJflIGlrABXe65TUA9Mr2tIClDMxH0",
  authDomain: "bloodbridge-3f25e.firebaseapp.com",
  projectId: "bloodbridge-3f25e",
  storageBucket: "bloodbridge-3f25e.appspot.com", // ✅ Corrected bucket domain
  messagingSenderId: "192881021185",
  appId: "1:192881021185:web:503c5a3c1afb6b1c2b0b00",
  measurementId: "G-GCGJR69LNL"
};

// 🔧 Initialize Firebase Services
const app = initializeApp(firebaseConfig);
const db = getFirestore(app);
const auth = getAuth(app);
getAnalytics(app);

// 📅 Format Firestore Timestamp
const formatDate = (timestamp) => {
  if (!timestamp) return "--";
  const date = timestamp.toDate ? timestamp.toDate() : timestamp;
  return new Date(date).toLocaleDateString("en-US", {
    year: "numeric",
    month: "short",
    day: "numeric"
  });
};

// 👤 Auth State Listener
onAuthStateChanged(auth, async (user) => {
  if (!user) {
    setTimeout(() => {
      window.location.href = "/login.html";
    }, 150);
    return;
  }

  try {
    const userRef = doc(db, "users", user.uid);
    const userSnap = await getDoc(userRef);

    if (!userSnap.exists()) {
      showAlert("User data not found.", "danger");
      return;
    }

    const data = userSnap.data();
    const fields = {
      fullName: data.fullName,
      email: data.email,
      phoneNumber: data.phoneNumber || "Not Provided",
      bloodGroup: data.bloodGroup || "Unknown",
      dob: formatDate(data.dateOfBirth),
      nationalId: data.nationalIdNumber || "N/A",
      emergencyName: data.emergencyContactName || "N/A",
      emergencyPhone: data.emergencyContactPhone || "N/A",
      nextDonation: formatDate(data.nextDonationDate),
      tipNextDonation: formatDate(data.nextDonationDate),
      lastDonation: formatDate(data.lastDonationDate),
      nextAppointment: formatDate(data.nextAppointment),
      profilePercent: `${data.profileCompletion || 0}%`
    };

    Object.entries(fields).forEach(([id, value]) => setText(id, value));

    const progressBar = document.getElementById("profileProgressBar");
    if (progressBar) {
      const percent = data.profileCompletion || 0;
      progressBar.style.width = `${percent}%`;
      progressBar.textContent = `${percent}%`;
    }
  } catch (err) {
    showAlert("Something went wrong while loading your profile.", "danger");
    console.error(err);
  }
});

// 🧩 Utility Functions
function setText(id, value) {
  const el = document.getElementById(id);
  if (el) el.textContent = value;
}

function showAlert(message, type = "success") {
  const alertBox = document.getElementById("alertBox");
  if (alertBox) {
    alertBox.className = `alert alert-${type}`;
    alertBox.textContent = message;
    alertBox.classList.remove("d-none");
  }
}