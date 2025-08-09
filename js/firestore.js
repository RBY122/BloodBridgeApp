// Import the functions you need from the SDKs you need
import { initializeApp } from "firebase/app";
import { getAnalytics } from "firebase/analytics";
// TODO: Add SDKs for Firebase products that you want to use
// https://firebase.google.com/docs/web/setup#available-libraries

// Your web app's Firebase configuration
// For Firebase JS SDK v7.20.0 and later, measurementId is optional
const firebaseConfig = {
  apiKey: "AIzaSyCkxDJflIGlrABXe65TUA9Mr2tIClDMxH0",
  authDomain: "bloodbridge-3f25e.firebaseapp.com",
  projectId: "bloodbridge-3f25e",
  storageBucket: "bloodbridge-3f25e.appspot.com", // ✅ Corrected
  messagingSenderId: "192881021185",
  appId: "1:192881021185:web:6588421760928bdb2b0b00",
  measurementId: "G-W75RZG1BYV"
};

// Initialize Firebase
const app = initializeApp(firebaseConfig);
const analytics = getAnalytics(app);


// Sign Up Handler
document.getElementById('signup-form').addEventListener('submit', (e) => {
  e.preventDefault();
  const email = document.getElementById('signup-email').value;
  const password = document.getElementById('signup-password').value;

  createUserWithEmailAndPassword(auth, email, password)
    .then((userCredential) => {
      document.getElementById('auth-feedback').textContent = `✅ Welcome, ${userCredential.user.email}! Account created.`;
    })
    .catch((error) => {
      document.getElementById('auth-feedback').textContent = `❌ Sign-up error: ${error.message}`;
    });
});

// Sign In Handler
document.getElementById('signin-form').addEventListener('submit', (e) => {
  e.preventDefault();
  const email = document.getElementById('signin-email').value;
  const password = document.getElementById('signin-password').value;

  signInWithEmailAndPassword(auth, email, password)
    .then((userCredential) => {
      document.getElementById('auth-feedback').textContent = `✅ Welcome back, ${userCredential.user.email}!`;
    })
    .catch((error) => {
      document.getElementById('auth-feedback').textContent = `❌ Sign-in error: ${error.message}`;
    });
});


//signOut
import { signOut } from "https://www.gstatic.com/firebasejs/10.5.0/firebase-auth.js";

document.getElementById("signOutBtn").addEventListener("click", () => {
  signOut(auth)
    .then(() => {
      window.location.href = "../Pages/SignIn.html";
    })
    .catch((error) => {
      alert("❌ Sign-out failed: " + error.message);
    });
});

loadDonationHistory(user.uid);

//trials//

import { ref, uploadBytes, getStorage, connectStorageEmulator } from "firebase/storage";

const storage = getStorage(app);
connectStorageEmulator(storage, "127.0.0.1", 9199); // Optional for local testing

const storageRef = ref(storage, 'uploads/example.txt');
const file = new Blob(['Hello BloodBridge!'], { type: 'text/plain' });

uploadBytes(storageRef, file)
  .then((snapshot) => {
    console.log('✅ File uploaded successfully:', snapshot);
  })
  .catch((error) => {
    console.error('❌ Upload failed:', error);
  });

  