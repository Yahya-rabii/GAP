var notifBtn = document.querySelector('.notification-drop');

notifBtn.addEventListener('click', function () {
    var notificationList = this.querySelector('ul');
    notificationList.style.display = notificationList.style.display === 'none' ? 'block' : 'none';
});
