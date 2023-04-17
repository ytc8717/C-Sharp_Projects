import { useState } from 'react';
import RankItems from './RankItems';


const RankItemsContainer = ({ dataType, imgArr }) => {
    const programmingLanguageLocalStorageKey = "programmingLanguages";
    const gameLocalStorageKey = "games";

    var localStorageKey = "";

    const [programmingLanguageItems, setProgrammingLanguagesItems] = useState(JSON.parse(localStorage.getItem(programmingLanguageLocalStorageKey)));
    const [gameItems, setGameItems] = useState(JSON.parse(localStorage.getItem(gameLocalStorageKey)));

    var data = [];
    var setFunc = null;

    if (dataType === 1) {
        data = gameItems;
        setFunc = setGameItems;
        localStorageKey = gameLocalStorageKey;
    }
    else if (dataType === 2) {
        data = programmingLanguageItems;
        setFunc = setProgrammingLanguagesItems;
        localStorageKey = programmingLanguageLocalStorageKey;
    }

    return (
        <RankItems items={data} setItems={setFunc} dataType={dataType} imgArr={imgArr} localStorageKey={localStorageKey} />
    )

}

export default RankItemsContainer;