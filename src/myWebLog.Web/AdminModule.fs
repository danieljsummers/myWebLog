﻿namespace myWebLog

open myWebLog.Data.WebLog
open myWebLog.Entities
open Nancy
open RethinkDb.Driver.Net

/// Handle /admin routes
type AdminModule(conn : IConnection) as this =
  inherit NancyModule("/admin")
  
  do
    this.Get.["/"] <- fun _ -> upcast this.Dashboard ()

  /// Admin dashboard
  member this.Dashboard () =
    this.RequiresAccessLevel AuthorizationLevel.Administrator
    let model = DashboardModel(this.Context, this.WebLog, findDashboardCounts conn this.WebLog.id)
    model.pageTitle  <- Resources.Dashboard
    this.View.["admin/dashboard", model]
