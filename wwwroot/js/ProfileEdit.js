// Update the chosen profile picture when a new image is selected
function updateChosenProfilePicture(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            document.getElementById('profile-picture').src = e.target.result;
            // Set the chosen profile picture to the hidden input field
            document.getElementById('chosen-profile-picture').value = e.target.result;
        };
        reader.readAsDataURL(input.files[0]);
    }
}

// Handle click on the circular profile picture
document.getElementById('profile-picture').addEventListener('click', function () {
    document.getElementById('ProfilePicture').click();
});