const btnChat = document.getElementById("btnChat");
const chatBox = document.getElementById("chatBox");

btnChat.addEventListener("click", () => {
    chatBox.style.display = "block";
    btnChat.style.display = "none";
});

function cerrarChat() {
    chatBox.style.display = "none";
    btnChat.style.display = "block";
}