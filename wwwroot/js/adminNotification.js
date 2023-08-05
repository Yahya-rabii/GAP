var notifBtn = document.querySelector('.adnotification-drop');

notifBtn.addEventListener('click', function () {
    var notificationList = this.querySelector('ul');
    notificationList.style.display = notificationList.style.display === 'none' ? 'block' : 'none';
});
