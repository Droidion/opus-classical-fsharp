/// SQL requests as string literals
module SiteSaturn.Database.SqlRequests

/// Select users with the given login
let userByLogin = "select login, password from users where login = @Login"

/// Select all labels
let labels = "select id, name from labels"

/// Select Composer by its slug name
let composerBySlug = "
    select json_build_object(
           'id', c.id,
           'lastName', c.last_name,
           'firstName', c.first_name,
           'yearBorn', c.year_born,
           'yearDied', c.year_died,
           'countries', json_agg(c2.name),
           'slug', c.slug,
           'enabled', c.enabled,
           'wikipediaLink', c.wikipedia_link,
           'imslpLink', c.imslp_link) json
    from composers as c
             join composers_countries cc on c.id = cc.composer_id
             join countries c2 on cc.country_id = c2.id
    where c.slug = @ComposerSlug
    group by c.id, c.last_name, c.first_name, c.year_born, c.year_died
    order by c.last_name"

/// Fuzzy search composers by last name with limiting results
let searchComposersByLastName = "
    select id, last_name, similarity(last_name, @SearchQuery) as last_name_score
    from composers
    where last_name % @SearchQuery
    order by last_name_score desc
    limit @Limit"

/// Select composers grouped by music periods
let periodsAndComposers = "
    select json_agg(json_build_object(
                            'id', p.id,
                            'name', p.name,
                            'yearStart', p.year_start,
                            'yearEnd', p.year_end,
                            'slug', p.slug,
                            'composers', p.composers
                        ) order by p.year_start) as Json
    from (select p.id,
                 p.name,
                 p.year_start,
                 p.year_end,
                 p.slug,
                 json_agg(json_build_object(
                                  'id', c.id,
                                  'lastName', c.last_name,
                                  'firstName', c.first_name,
                                  'yearBorn', c.year_born,
                                  'yearDied', c.year_died,
                                  'countries', c.countries,
                                  'slug', c.slug,
                                  'enabled', c.enabled,
                                  'wikipediaLink', c.wikipedia_link,
                                  'imslpLink', c.imslp_link) order by c.last_name) composers
          from periods p
                   join (select c.id,
                                c.last_name,
                                c.first_name,
                                json_agg(c2.name) countries,
                                c.year_born,
                                c.year_died,
                                c.slug,
                                c.enabled,
                                c.wikipedia_link,
                                c.imslp_link,
                                c.period_id
                         from composers c
                                  join composers_countries cc on c.id = cc.composer_id
                                  join countries c2 on cc.country_id = c2.id
                         group by c.id, c.last_name, c.first_name, c.year_born, c.year_died, c.slug, c.wikipedia_link,
                                  c.imslp_link, c.period_id
          ) c on p.id = c.period_id
          group by p.id, p.name, p.year_start, p.year_end, p.slug) p"

/// Select work by its id
let workById = "
    select w.id,
           w.title,
           w.year_start yearStart,
           w.year_finish yearFinish,
           w.average_minutes averageMinutes,
           c.name catalogueName,
           w.catalogue_number catalogueNumber,
           w.catalogue_postfix cataloguePostfix,
           k.name as key,
           w.no,
           w.nickname
    from works w
             left join catalogues c on w.catalogue_id = c.id
             left join keys k on w.key_id = k.id
    where w.id = @Id"

/// Select works by their parent work Id
let childWorks = @"
    select w.id,
           w.title,
           w.year_start yearStart,
           w.year_finish yearFinish,
           w.average_minutes averageMinutes,
           c.name as catalogueName,
           w.catalogue_number catalogueNumber,
           w.catalogue_postfix cataloguePostfix,
           k.name as key,
           w.no,
           w.nickname
    from works w
             left join catalogues c on w.catalogue_id = c.id
             left join keys k on w.key_id = k.id
    where w.parent_work_id = @Id
    order by sort, year_finish, no, catalogue_number, catalogue_postfix, nickname"

/// Select works grouped by genres by composer Id
let genresAndWorksByComposer = "
    select json_agg(json_build_object(
                            'name', g.name,
                            'icon', g.icon,
                            'works', g.works
                        ) order by g.name) json
    from (select g.name,
                 g.icon,
                 json_agg(json_build_object(
                                  'id', w.id,
                                  'title', w.title,
                                  'yearStart', w.year_start,
                                  'yearFinish', w.year_finish,
                                  'averageMinutes', w.average_minutes,
                                  'catalogueName', c.name,
                                  'catalogueNumber', w.catalogue_number,
                                  'cataloguePostfix', w.catalogue_postfix,
                                  'key', k.name,
                                  'no', w.no,
                                  'nickname', w.nickname
                              ) order by w.sort, w.year_finish, w.no, w.catalogue_number, w.nickname) works
          from works w
                   join works_genres wg on w.id = wg.work_id
                   join genres g on wg.genre_id = g.id
                   left join catalogues c on w.catalogue_id = c.id
                   left join keys k on w.key_id = k.id
          where w.composer_id = @ComposerId
          group by g.name,
                   g.icon) g"

/// Select recordings of certain work
let recordingsByWork = "
    select jsonb_agg(jsonb_build_object(
                            'id', w.Id,
                            'coverName', w.cover_name,
                            'yearStart', w.year_start,
                            'yearFinish', w.year_finish,
                            'performers', w.performers,
                            'label', w.name,
                            'length', w.length,
                            'streamers', w.streamers
                        )) json
    from (select r.id,
           jsonb_agg(jsonb_build_object(
                   'firstName', p.first_name,
                   'lastName', p.last_name,
                   'priority', pri.priority,
                   'instrument', i.name
               ) order by pri.priority, p.last_name) performers,
           r.cover_name,
           r.year_start,
           r.year_finish,
           r.name,
           r.length,
           r.streamers
    from (
             select r.id,
                    r.cover_name,
                    r.year_start,
                    r.year_finish,
                    l.name,
                    r.length,
                    jsonb_agg(jsonb_build_object('name', s.name,
                                               'icon', s.icon_name,
                                               'link', rs.link,
                                               'prefix', s.app_prefix) order by s.name) streamers
             from recordings r
                      left join labels l on r.label_id = l.id
                      join recordings_streamers rs on r.id = rs.recording_id
                      join streamers s on rs.streamer_id = s.id
             where r.work_id = @WorkId
             group by r.id,
                      r.cover_name,
                      r.year_start,
                      r.year_finish,
                      l.name,
                      r.length
         ) r
             join performers_recordings_instruments pri on r.id = pri.recording_id
             join performers p on pri.performer_id = p.id
             join instruments i on pri.instrument_id = i.id
    group by r.id,
             r.cover_name,
             r.year_start,
             r.year_finish,
             r.name,
             r.length,
             r.streamers
    order by r.year_finish) w"
