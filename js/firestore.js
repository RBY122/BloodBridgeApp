import { initializeApp } from "firebase/app";
import { getFirestore, collection, addDoc, getDocs } from "firebase/firestore";

const firebaseConfig = {
  apiKey: "YOUR_API_KEY",
  projectId: "YOUR_PROJECT_ID",
  ...
};

const app = initializeApp(firebaseConfig);
const db = getFirestore(app);

// Example: Add a donation
export async function addDonation(data) {
  await addDoc(collection(db, "donations"), data);
}

// Example: Fetch donors
export async function getDonors() {
  const snapshot = await getDocs(collection(db, "donors"));
  return snapshot.docs.map(doc => doc.data());
}
