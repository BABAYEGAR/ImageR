/*
Author       : Hash Theme.
Template Name: Korna - One page Portfolio Creative Template
Version      : 1.0
*/
/*=============================================
Table Of Contents
================================================

1. PRELOADER JS
2. BOOTSTRAP TOOLTIP
3. START MENU JS
4. START SLIDER JS
5. START SERVICES SLIDER
6. START TESTIMONIAL SLIDER
7. START PARTNER LOGO
8. START COUNTDOWN JS
9. SECTIONS BACKGROUNDS
10. START GOOGLE MAP
11. START MIXITUP JS
12. START WOW ANIMATION JS


Table Of Contents end
 ================================================
 */
(function($) {
    'use strict';

    jQuery(document).on('ready', function() {

        /* 1. PRELOADER JS */

        $(window).on('load', function() {
            $('.loadscreen').fadeOut();
            $('.preloader').delay(350).fadeOut('slow');
        });

        /*END PRELOADER JS*/


        /* 2. BOOTSTRAP TOOLTIP  */

        $('[data-toggle="tooltip"]').tooltip();

        /* END BOOTSTRAP TOOLTIP  */


        /* 3. START MENU JS */

        $('a.page-scroll').on('click', function(e) {
            var anchor = $(this);
            $('html, body').stop().animate({
                scrollTop: $(anchor.attr('href')).offset().top - 50
            }, 1500);
            e.preventDefault();
        });

        $(window).on('scroll', function() {
            if ($(this).scrollTop() > 100) {
                $('.main-menu').addClass('menu-animation');
            } else {
                $('.main-menu').removeClass('menu-animation');
            }
        });

        $(document).on('click', '.navbar-collapse.in', function(e) {
            if ($(e.target).is('a') && $(e.target).attr('class') != 'dropdown-toggle') {
                $(this).collapse('hide');
            }
        });

        /* END MENU JS */

        /* 4. START SLIDER JS */
		$('.carousel').carousel({
			interval: 4000,
			pause:'false',
		});



        /* 5. START SERVICES SLIDER */

        $('.services-slider').owlCarousel({
            autoPlay: false, //Set AutoPlay to 3 seconds
            items: 3,
            itemsDesktop: [1199, 3],
            itemsDesktopSmall: [979, 2],
            itemsTablet: [768, 1],
            pagination: true
        });

        /* END SERVICES SLIDER */

        /* 6. START TESTIMONIAL SLIDER */

        $('.client-testimonial').owlCarousel({
            autoPlay: true, //Set AutoPlay to 3 seconds
            items: 2,
            itemsDesktop: [1199, 2],
            itemsDesktopSmall: [979, 2],
            itemsTablet: [768, 1],
            pagination: true
        });

        /*END TESTIMONIAL SLIDER */

        /* 7. START PARTNER LOGO */
        $('.client').owlCarousel({
            autoPlay: true, //Set AutoPlay to 3 seconds
            items: 5,
            itemsDesktop: [1199, 3],
            itemsDesktopSmall: [979, 3],
            pagination: false
        });

        /* END PARTNER LOGO */

        /* 8. START COUNTDOWN JS */
        $('.counter-section').on('inview', function(event, visible, visiblePartX, visiblePartY) {
            if (visible) {
                $(this).find('.timer').each(function() {
                    var $this = $(this);
                    $({
                        Counter: 0
                    }).animate({
                        Counter: $this.text()
                    }, {
                        duration: 2000,
                        easing: 'swing',
                        step: function() {
                            $this.text(Math.ceil(this.Counter));
                        }
                    });
                });
                $(this).unbind('inview');
            }
        });
        /* END COUNTDOWN JS */


        /* 9. SECTIONS BACKGROUNDS */

        var pageSection = $("section");
        pageSection.each(function(indx) {

            if ($(this).attr("data-background")) {
                $(this).css("background-image", "url(" + $(this).data("background") + ")");
            }
        });

        /* END SECTIONS BACKGROUNDS */

        /* 10. START GOOGLE MAP */

        function initialize() {
            var mapOptions = {
                zoom: 11,
                scrollwheel: false,
                center: new google.maps.LatLng(40.7143528, -74.0059731)
            };
            var map = new google.maps.Map(document.getElementById('map'),
                mapOptions);
            var marker = new google.maps.Marker({
                position: new google.maps.LatLng(40.7143528, -74.0059731),
                animation: google.maps.Animation.BOUNCE,
                icon: 'assets/img/map-marker.png',
                map: map
            });
        }
        google.maps.event.addDomListener(window, 'load', initialize);

        /* END GOOGLE MAP */

    });

    /* 11. START MIXITUP JS */

    $('.portfoli-items').mixItUp();

    // jQuery Lightbox
    $('.venobox').venobox({
        numeratio: true,
        titleattr: 'data-title',
        infinigall: true
    });

    /* END MIXITUP JS */

    /* 12. START WOW ANIMATION JS */

    new WOW().init();

    /* END WOW ANIMATION JS */


})(jQuery);