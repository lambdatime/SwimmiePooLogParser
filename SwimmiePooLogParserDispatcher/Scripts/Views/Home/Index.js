$(function () {
    function droneViewModel(id, title, baseUrl, sortOrder, ownerViewModel) {
        this.Id = id;
        this.Title = ko.observable(title);
        this.BaseUrl = ko.observable(baseUrl);
        this.SortOrder = ko.observable(sortOrder);

        this.remove = function () { ownerViewModel.removeDrone(this.Id); };
        this.enableProcessing = function () { ownerViewModel.enableProcessing(this.Id); };
        this.abort = function () { ownerViewModel.abortProcessing(this.Id); };

        this.Status = ko.observable(new droneStatusIdledViewModel(this, "Not Available Yet", "0:0:0.00", ownerViewModel));
        this.StatusTooltipCssClass = "dronestatustip";

        this.DisableProcessing = ko.computed(function () {
            return this.Status().StatusName() == "Busy";
        }, this);
        this.DisableAbort = ko.computed(function () {
            return this.Status().StatusName() != "Busy";
        }, this);

        this.IsError = ko.computed(function () {
            return this.Status().IsError;
        }, this);
        this.IsBusy = ko.computed(function () {
            return this.Status().StatusName() == "Busy";
        }, this);
        this.IsIdle = ko.computed(function () {
            return this.Status().StatusName() == "Idle";
        }, this);
        this.IsAborted = ko.computed(function () {
            return this.Status().StatusName() == "Aborted";
        }, this);

        this.StatusRowClass = ko.computed(function () {
            if (this.IsIdle()) return "success";
            if (this.IsBusy()) return "warning";
            if (this.IsAborted()) return "error";

            return "error";
        }, this);
        
        var self = this;
    }

    function droneListViewModel() {
        var self = this;
        
        //self.hub = $.connection.Drones;
        self.hub = $.connection.Drones;
        self.drones = ko.observableArray([]);
        self.newDroneText = ko.observable();
        self.newDroneBaseUrl = ko.observable();
        self.listCssClass = "dronelist";

        var drones = self.drones;

        var notify = true;

        self.drones.subscribe(function (newValue) {
        });

        //Initializes the view model
        self.init = function () {
            self.hub.server.getAll();
        };

        self.hub.client.droneAll = function (allItems) {

            var mappedItems = $.map(allItems, function (item) {
                return new droneViewModel(item.Id, item.Title,
                    item.BaseUrl, item.SortOrder, self);
            });

            drones(mappedItems);

            //fix sort orders (if necessary)
            //self.fixSolrCoreSortOrders();

            //$("#droneTable tbody").sortable({
            //    helper: fixHelper,
            //    update: function (event, ui) {
            //        var id = ui.item.first().attr('data-ref');
            //        var drone = ko.utils.arrayFilter(drones(), function (value) { return value.Id == id; })[0];
            //        var prevOrder = drone.SortOrder();
            //        var newIndex = ui.item.index();


            //        if (newIndex == prevOrder) return;
            //        var itemsInSortRange = null;
            //        var increment = -1;
            //        if (newIndex > prevOrder) {
            //            itemsInSortRange = ko.utils.arrayFilter(drones(), function (value) { return value.SortOrder() > prevOrder && value.SortOrder() <= newIndex; });
            //        } else {
            //            itemsInSortRange = ko.utils.arrayFilter(drones(), function (value) { return value.SortOrder() < prevOrder && value.SortOrder() >= newIndex; });
            //            increment = 1;
            //        }
            //        itemsInSortRange.forEach(function (item) {
            //            var newSortOrder = item.SortOrder() + increment;
            //            item.SortOrder(newSortOrder);
            //        });
            //        drone.SortOrder(newIndex);
            //        var droneCopy = itemsInSortRange.slice();
            //        droneCopy.push(drone);
            //        self.updateMassSortOrders(ko.toJS(droneCopy));
            //    }
            //}).disableSelection();
        };
        
        self.hub.client.droneAdded = function (sc) {
            drones.push(new droneViewModel(sc.DroneId, sc.Title, sc.BaseUrl, sc.SortOrder, self));
        };

        self.hub.client.droneRemoved = function (id) {
            var drone = ko.utils.arrayFilter(drones(), function (value) { return value.Id == id; })[0];
            drones.remove(drone);
        };

        self.hub.client.droneProcessingEnabled = function (sc) {
        };

        self.hub.client.droneAborting = function (sc) {
        };

        //Add
        self.addDrone = function () {
            var sc = { "Title": self.newDroneText(), "BaseUrl": self.newDroneBaseUrl() };
            self.hub.server.add(sc);
            //self.hub.server.add(sc).done(function () {
            //    if (!window.console)
            //        console = {
            //            log: function () {
            //            }
            //        };
            //    console.log('Success!');
            //}).fail(function (e) {
            //    window.console && console.warn(e);
            //});

            self.newDroneText("");
            self.newDroneBaseUrl("");
        };

        //Remove
        self.removeDrone = function (id) {
            self.hub.server.remove(id);
        };

        //EnableProcessing
        self.enableProcessing = function (id) {
            self.hub.server.enableProcessing(id);
        };

        //Abort
        self.abortProcessing = function (id) {
            self.hub.server.abort(id);
        };
    }

    function droneStatusBusyViewModel(drone, name, timeElapsed, totalProcessed, ownerViewModel) {
        this.StatusName = ko.observable(name);
        this.StatusAlt = ko.observable("Time elapsed: " + timeElapsed + "<br /> Processed: " + totalProcessed);
        this.TimeElapsed = ko.observable(timeElapsed);
        this.TotalProcessed = ko.observable(totalProcessed);
        this.IsError = false;
    }

    function droneStatusIdledViewModel(drone, name, timeTaken, ownerViewModel) {
        this.StatusName = ko.observable(name);
        if (timeTaken != "0:0:0.00")
            this.StatusAlt = ko.observable("Processing completed after " + timeTaken + ".");
        else {
            this.StatusAlt = ko.observable("");
        }
        this.IsError = false;
        this.TimeTaken = timeTaken;
    }

    function droneStatusAbortedViewModel(drone, name, timeTaken, ownerViewModel) {
        this.StatusName = ko.observable(name);
        this.StatusAlt = ko.observable("Processing aborted after " + timeTaken + ".");
        this.IsError = false;
        this.TimeTaken = timeTaken;
    }

    function droneErrorStatusViewModel(drone, name, errorMessage, ownerViewModel) {
        this.StatusName = ko.observable(name);
        this.StatusAlt = ko.observable(errorMessage);
        this.IsError = true;
        this.ErrorMessage = errorMessage;
    }
    
    var vm = new droneListViewModel();

    ko.applyBindings(vm);
    // Start the connection
    $.connection.hub.start().done(function () { vm.init(); });

    if (!console) { console = {}; console.log = function () { }; }

    // Return a helper with preserved width of cells
    var fixHelper = function (e, ui) {
        ui.children().each(function () {
            $(this).width($(this).width());
        });
        return ui;
    };
});