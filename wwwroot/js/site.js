// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var menuBtn = document.querySelector('.menu-btn');
var nav = document.querySelector('.sidenav');
var lineOne = document.querySelector('.sidenav .menu-btn .line--1');
var lineTwo = document.querySelector('.sidenav .menu-btn .line--2');
var lineThree = document.querySelector('.sidenav .menu-btn .line--3');
var link = document.querySelector('.sidenav .sidenav-links');
menuBtn.addEventListener('click', () => {
    nav.classList.toggle('sidenav-open');
    lineOne.classList.toggle('line-cross');
    lineTwo.classList.toggle('line-fade-out');
    lineThree.classList.toggle('line-cross');
    link.classList.toggle('fade-in');
})