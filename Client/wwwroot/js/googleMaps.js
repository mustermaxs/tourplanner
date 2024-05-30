function initMap() {
    // Create a map centered at a specific location
    var map = new google.maps.Map(document.getElementById('map'), {
        zoom: 7,
        center: {lat: 41.85, lng: -87.65}  // Default location
    });

    // Define the directions service and renderer
    var directionsService = new google.maps.DirectionsService();
    var directionsRenderer = new google.maps.DirectionsRenderer();

    // Link the directions renderer to the map
    directionsRenderer.setMap(map);

    // Define the start and end points of the route
    var start = 'Chicago, IL';
    var end = 'Los Angeles, CA';

    // Calculate the route
    directionsService.route(
        {
            origin: start,
            destination: end,
            travelMode: google.maps.TravelMode.DRIVING
        },
        function (response, status) {
            if (status === 'OK') {
                directionsRenderer.setDirections(response);
            } else {
                window.alert('Directions request failed due to ' + status);
            }
        }
    );
}