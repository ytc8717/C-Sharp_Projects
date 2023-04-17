import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import GameImageArr from "./components/GameImages";
import { Home } from "./components/Home";
import ProgrammingLanguageImageArr from "./components/ProgrammingLanguageImages";
import RankItemsContainer from "./components/RankItemsContainer";

const AppRoutes = [
    {
        index: true,
        element: <Home />
    },
    {
        path: '/counter',
        element: <Counter />
    },
    {
        path: '/fetch-data',
        element: <FetchData />
    }
    ,
    {
        path: '/rank-games',
        element: <RankItemsContainer dataType={1} imgArr={GameImageArr} />
    },
    ,
    {
        path: '/rank-programmingLanguages',
        element: <RankItemsContainer dataType={2} imgArr={ProgrammingLanguageImageArr} />
    }
];

export default AppRoutes;
