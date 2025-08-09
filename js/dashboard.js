import { collection, query, where, getDocs } from "https://www.gstatic.com/firebasejs/10.5.0/firebase-firestore.js";

async function loadDonationHistory(userId) {
  const donationsRef = collection(db, "donations");
  const q = query(donationsRef, where("userId", "==", userId));
  const querySnapshot = await getDocs(q);

  const donationList = document.getElementById("donationList");
  donationList.innerHTML = ""; // Clear previous content

  if (querySnapshot.empty) {
    donationList.innerHTML = "<p class='text-muted'>No donation history found.</p>";
    return;
  }

  querySnapshot.forEach((doc) => {
    const data = doc.data();
    const item = document.createElement("div");
    item.className = "card mb-2";
    item.innerHTML = `
      <div class="card-body">
        <h5 class="card-title">${data.bloodType} Donation</h5>
        <p class="card-text">
          <strong>Date:</strong> ${data.date}<br>
          <strong>Location:</strong> ${data.location}<br>
          <strong>Status:</strong> ${data.status}
        </p>
      </div>
    `;
    donationList.appendChild(item);
  });
}