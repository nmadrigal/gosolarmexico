GosolarApp = {};
GosolarApp.Home = {};

(function (g, $) {
    var self = this;    
    var currentPosition = new Object();
    var address;
    var addressMap;
    var newReferralAddress;
    g.Init = function () {       
        g.GetLocation();
        //$(".main-section .main-menu a").unbind("click");
        //$(".main-section .main-menu a").bind("click", function () {
        //    $(".tab-bar .left-small .menu-icon").hide();
        //});

        $(".forgot-password").bind("click", function () {
            $('#forgotPasswordModal').foundation('reveal', 'open');
        });
        $(".change-password").bind("click", function () {
            $('#changePasswordModal').foundation('reveal', 'open');
        });

        $(".get-address").unbind("click");
        $(".get-address").bind("click", function (e) {            
            g.InitialMapGetAddress();
            $('#getAddressModal').foundation('reveal', 'open');
            
            $(document).on('opened.fndtn.reveal', '[data-reveal]', function () {               
                google.maps.event.trigger(addressMap, "resize");
                addressMap.setCenter(address);
                $(".get-address-data").unbind("click");
                $(".get-address-data").bind("click", function (ev) {
                    g.getAddressFields();
                    $('#getAddressModal').foundation('reveal', 'close');
                    ev.stopPropagation();
                });
                $(".btn-close-modal").unbind("click");
                $(".btn-close-modal").bind("click", function (ev) {
                    $('#getAddressModal').foundation('reveal', 'close');
                });
            });
            
        });
    };

    g.GetLocation = function () {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(g.ShowPosition);
        } else {
            var alert = '<div data-alert class="alert-box alert">Geolocation is not supported by this browser.<a href="#" class="close">&times;</a></div>';
            $(".container").prepend(alert);
        }
    };

    g.ShowPosition = function (position) {
        currentPosition.lat = position.coords.latitude;
        currentPosition.lng = position.coords.longitude;
    };

    g.InitialMapGetAddress = function () {
        address = g.InitialMapPoint(currentPosition.lat, currentPosition.lng);
        //address = new google.maps.LatLng(currentPosition.lat, currentPosition.lng);
        var optionsAddress = {
            center: address,
            zoom: 14
            //mapTypeControl: false,
            //streetViewControl: false
        };
        addressMap = g.CreateMap(optionsAddress, "map-canvas-address");
        //addressMap = new google.maps.Map(document.getElementById('map-canvas-address'), optionsAddress);
        var markOptions = {
            position: address,
            map: addressMap,
            draggable: true
        };
        var point = g.CreateMarker(markOptions);
        google.maps.event.addListener(point, "dragend", function (e) {
            var geoCodergetDirection = new google.maps.Geocoder();
            if (geoCodergetDirection) {
                geoCodergetDirection.geocode({ location: e.latLng }, function (results, status) {
                    newReferralAddress = getAddressData(results, status);
                });
                $(".direccion").attr("data-location", 1);
            }

        });
        function getAddressData(results, status) {
            var _container = $("#getAddressModal");
            var newData = {};
            if (status == google.maps.GeocoderStatus.OK) {
                for (x = 0; x < results.length; x++) {
                    var obj = results[x];
                    newData.latitude = results[x].geometry.location.lat();
                    newData.longitude = results[x].geometry.location.lng();
                    //newData.formattedAddress = results[x].formatted_address;
                    for (i = 0; i < obj.address_components.length; i++) {
                        var o = obj.address_components[i];
                        switch (o.types[0]) {
                            case "street_number":
                                newData.streetnumber = o.long_name;
                                break;
                            case "route":
                                newData.street = o.long_name;
                                break;                            
                            case "sublocality_level_1":
                                newData.neighborhood = o.long_name;                           
                            case "locality":
                                newData.city = o.long_name;
                                break;
                            case "administrative_area_level_1":
                                newData.state = o.long_name;
                                break;
                            case "country":
                                newData.country = o.long_name;
                                break;
                        }
                    };
                };

                latLng = results[0].geometry.location.lat() + "," + results[0].geometry.location.lng();
                console.log(newData);
                _container.find("#Street").val(newData.street);// + ", " + newData.streetnumber + ", " + newData.neighborhood + ", " + newData.city + ", " + newData.state + ", " + newData.country);
                 //_container.find(".latLng").val(newData.latitude + ", " + newData.longitude);
                _container.find("#ExternalNum").val(newData.streetnumber);                
                _container.find("#ResidentialArea").val(newData.neighborhood);
                _container.find("#City").val(newData.city);
                _container.find("#State").val(newData.state);
                //_container.find("#country").val(newData.country);
                return newData;
            }

        }
    };  

    g.InitialMapPoint = function (lat, lng) {
        return new google.maps.LatLng(lat, lng);
    };

    g.CreateMap = function (mapOptions, idElement) {
        return new google.maps.Map(document.getElementById(idElement), mapOptions);
    };

    g.CreateMarker = function (markOptions) {
        var marker = new google.maps.Marker({
            position: markOptions.position,
            map: markOptions.map,
            draggable: markOptions.draggable
        });

        marker.setMap(markOptions.map);
        return marker;
    };

    g.getAddressFields = function () {        
        if (newReferralAddress) {
            var _container = $(".main-section form");
            var addressElement = $("<p class='panel address-data'>");
            addressElement.append(newReferralAddress.street + " ");
            addressElement.append(newReferralAddress.streetnumber + ", ");
            addressElement.append(newReferralAddress.neighborhood + ", ");
            addressElement.append(newReferralAddress.city + ", ");
            addressElement.append(newReferralAddress.state + ", ");
            addressElement.append(newReferralAddress.country);
            if ($(".address-data").length > 0)
                $(".address-data").remove();
            _container.find(".get-address").before(addressElement);
        }
    };

})(window.GosolarApp.Home = window.GosolarApp.Home || {}, jQuery);
