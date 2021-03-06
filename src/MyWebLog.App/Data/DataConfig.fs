﻿namespace MyWebLog.Data.RethinkDB

open RethinkDb.Driver
open RethinkDb.Driver.Net
open Newtonsoft.Json

/// Data configuration
type DataConfig =
  { /// The hostname for the RethinkDB server
    [<JsonProperty("hostname")>]
    Hostname : string
    /// The port for the RethinkDB server
    [<JsonProperty("port")>]
    Port : int
    /// The authorization key to use when connecting to the server
    [<JsonProperty("authKey")>]
    AuthKey : string
    /// How long an attempt to connect to the server should wait before giving up
    [<JsonProperty("timeout")>]
    Timeout : int
    /// The name of the default database to use on the connection
    [<JsonProperty("database")>]
    Database : string
    /// A connection to the RethinkDB server using the configuration in this object
    [<JsonIgnore>]
    Conn : IConnection }
with
  /// Use RethinkDB defaults for non-provided options, and connect to the server
  static member Connect config =
    let host cfg = match cfg.Hostname with null -> { cfg with Hostname = RethinkDBConstants.DefaultHostname } | _ -> cfg
    let port cfg = match cfg.Port with 0 -> { cfg with Port = RethinkDBConstants.DefaultPort } | _ -> cfg
    let auth cfg = match cfg.AuthKey with null -> { cfg with AuthKey = RethinkDBConstants.DefaultAuthkey } | _ -> cfg
    let timeout cfg = match cfg.Timeout with 0 -> { cfg with Timeout = RethinkDBConstants.DefaultTimeout } | _ -> cfg
    let db cfg = match cfg.Database with null -> { cfg with Database = RethinkDBConstants.DefaultDbName } | _ -> cfg
    let connect cfg =
      { cfg with Conn = RethinkDB.R.Connection()
                          .Hostname(cfg.Hostname)
                          .Port(cfg.Port)
                          .AuthKey(cfg.AuthKey)
                          .Db(cfg.Database)
                          .Timeout(cfg.Timeout)
                          .Connect () }
    (host >> port >> auth >> timeout >> db >> connect) config
