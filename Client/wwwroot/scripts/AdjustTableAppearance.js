function adjustTableCellOverflow()
{
    clipCellOverflow();
    
    window.onresize = debounce(() => {
        clipCellOverflow();
    }, 500);
}

function highlightSearchResults(searchTerm) {
    const escapedSearchTerm = searchTerm.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
    const searchTermPattern = new RegExp(`(${escapedSearchTerm})`, 'gmi');
    const highlightedSubstitutePattern = '<span style="background: yellow">$1</span>';
    
    const tables = document.querySelectorAll("table");

    tables.forEach(table => {
        let cells = table.querySelectorAll("td");
        
        cells.forEach(cell => {
            let content = cell.innerHTML;
            
            if (searchTermPattern.test(content)) {
                cell.innerHTML = content.replace(searchTermPattern, highlightedSubstitutePattern);
            }
        });
    });
}

function debounce(cb, timeout = 300){
    let timer;
    return (...args) => {
        clearTimeout(timer);
        timer = setTimeout(() => { cb.apply(this, args); }, timeout);
    };
}

function clipCellOverflow()
{
    const maxNbrOfCharsInCell = getMaxNbrOfChars(getScreenWidth());
    const tableCells = document.querySelectorAll("td");

    tableCells.forEach(cell => {
        if (cell.textContent.length > maxNbrOfCharsInCell)
        {
            cell.textContent = cell.textContent.substring(0, maxNbrOfCharsInCell - 3);
            cell.textContent += "...";
        }
    })
}

function getMaxNbrOfChars(screenWidth)
{
    const MIN_SCREEN_WIDTH = 1200;
    const MIN_CHAR_COUNT_SCREEN_LARGE = 250;
    const MIN_CHAR_COUNT_SCREEN_SMALL = 150;

    return getScreenWidth() < 1200 ? 150 : 250; 
}

function getScreenWidth()
{
    return window.innerWidth
        || document.documentElement.clientWidth
        || document.body.clientWidth;
}