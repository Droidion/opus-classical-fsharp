/// SQL requests as string literals
module Site.Database.SqlRequests

/// Select users with the given login
let userByLogin = "select login, password from users where login = @Login"

/// Select all labels
let labels = "select id, name from labels"

/// Select Composer by its slug name
let composerBySlug = "select composer_by_slug(@ComposerSlug) as json"

/// Fuzzy search composers by last name with limiting results
let searchComposersByLastName = "select id, first_name, last_name, slug, last_name_score from search_composers_by_last_name(@SearchQuery, @Limit)"

/// Select composers grouped by music periods
let periodsAndComposers = "select json from periods_composers"

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
let genresAndWorksByComposer = "select genres_and_works_by_composer(@ComposerId) as json"

/// Select recordings of certain work
let recordingsByWork = "select recordings_by_work(@WorkId) as json"
